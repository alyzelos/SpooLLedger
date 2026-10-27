import { CurrencyPipe, DatePipe, NgStyle } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormArray, FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { ProgressBarModule } from 'primeng/progressbar';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';
import { Customer, CustomersService } from '../customers/customers.service';
import { FilamentType, FilamentTypesService } from '../filament-types/filament-types.service';
import { FilamentStock, FilamentStockService } from '../filament-stock/filament-stock.service';
import { Item, ItemsService } from '../items/items.service';
import { Pricing, PricingService } from '../pricing/pricing.service';
import { NewOrderLine, Order, OrdersService } from './orders.service';

@Component({
  selector: 'app-orders',
  imports: [
    ButtonModule,
    CardModule,
    ConfirmDialogModule,
    CurrencyPipe,
    NgStyle,
    DatePipe,
    DialogModule,
    FormsModule,
    InputNumberModule,
    ProgressBarModule,
    InputTextModule,
    ReactiveFormsModule,
    SelectModule,
    TableModule,
  ],
  providers: [ConfirmationService],
  templateUrl: './orders.html',
  styleUrl: './orders.css',
})
export class OrdersPage implements OnInit {
  private readonly ordersService = inject(OrdersService);
  private readonly customersService = inject(CustomersService);
  private readonly itemsService = inject(ItemsService);
  private readonly filamentTypesService = inject(FilamentTypesService);
  private readonly filamentStockService = inject(FilamentStockService);
  private readonly pricingService = inject(PricingService);
  private readonly fb = inject(FormBuilder);
  private readonly confirmationService = inject(ConfirmationService);

  protected readonly statuses = ['Pending', 'Printing', 'Shipped', 'Confirmed payment', 'Cancelled'];
  protected readonly lineCompleted = signal(0);

  protected readonly orders = signal<Order[] | null>(null);
  protected readonly customers = signal<Customer[]>([]);
  protected readonly items = signal<Item[]>([]);
  protected readonly error = signal<string | null>(null);
  protected readonly dialogVisible = signal(false);
  protected readonly customerDialogVisible = signal(false);
  protected readonly saving = signal(false);
  protected readonly cancelStockVisible = signal(false);
  protected readonly cancelStock = signal<{ type: string; color: string; used: number; returned: number }[]>([]);
  protected readonly saveError = signal<string | null>(null);
  protected readonly customerError = signal<string | null>(null);
  protected readonly editing = signal<Order | null>(null);
  protected readonly filamentTypes = signal<FilamentType[]>([]);
  protected readonly filamentColors = signal<FilamentType[]>([]);
  protected readonly pricings = signal<Pricing[]>([]);
  protected readonly filamentStock = signal<FilamentStock[]>([]);
  protected readonly lineItemId = signal<number | null>(null);
  protected readonly lineFilamentType = signal<string | null>(null);
  protected readonly lineColor = signal<string | null>(null);
  protected readonly lineQuantity = signal(1);
  protected readonly draftOrder = signal<number | null>(null);
  protected readonly lineEdit = signal<{ orderNumber: number; itemId: number; filamentType: string; color: string } | null>(null);
  protected readonly savingLine = signal(false);
  protected readonly lineError = signal<string | null>(null);
  protected readonly expanded: Record<string, boolean> = {};
  protected readonly form = this.fb.group({
    customerId: this.fb.control<number | null>(null, Validators.required),
    status: this.fb.nonNullable.control('Pending', Validators.required),
    pricingName: this.fb.nonNullable.control('Standard', Validators.required),
    lines: this.fb.array([this.lineGroup()]),
  });

  protected readonly customerForm = this.fb.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
    phoneNumber: ['', [Validators.required, Validators.maxLength(30)]],
  });

  ngOnInit(): void {
    this.load();
    this.customersService.getAll().subscribe({
      next: (response) => this.customers.set(response),
    });
    this.itemsService.getAll().subscribe({
      next: (response) => this.items.set(response),
    });
    this.filamentTypesService.getAll().subscribe({
      next: (response) => this.filamentTypes.set(response),
    });
    this.filamentStockService.getColors().subscribe({
      next: (response) => this.filamentColors.set(response),
    });
    this.pricingService.getAll().subscribe({
      next: (response) => this.pricings.set(response),
    });
  }

  protected get lines(): FormArray {
    return this.form.controls.lines;
  }

  protected add(): void {
    this.editing.set(null);
    this.form.reset();
    this.form.controls.status.setValue('Pending');
    this.form.controls.pricingName.setValue('Standard');
    this.lines.clear();
    this.saveError.set(null);
    this.dialogVisible.set(true);
  }

  protected edit(order: Order): void {
    this.editing.set(order);
    this.lines.clear();
    this.form.controls.customerId.setValue(this.customerIdFor(order));
    this.form.controls.status.setValue(order.status ?? 'Pending');
    this.form.controls.pricingName.setValue(order.pricingName || 'Standard');
    this.saveError.set(null);
    this.dialogVisible.set(true);
  }

  protected remove(order: Order): void {
    this.confirmationService.confirm({
      header: 'Delete order',
      message: `Delete order #${order.orderNumber}?`,
      icon: 'pi pi-trash',
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      accept: () =>
        this.ordersService.delete(order.orderNumber).subscribe({
          next: () => this.load(),
          error: () => this.error.set('Could not delete the order.'),
        }),
    });
  }

  protected addLine(): void {
    this.lines.push(this.lineGroup());
  }

  protected removeLine(index: number): void {
    if (this.lines.length === 1) {
      return;
    }
    this.lines.removeAt(index);
  }

  protected openCustomer(): void {
    this.customerForm.reset();
    this.customerError.set(null);
    this.customerDialogVisible.set(true);
  }

  protected saveCustomer(): void {
    if (this.customerForm.invalid) {
      this.customerForm.markAllAsTouched();
      return;
    }

    this.customersService.create({ ...this.customerForm.getRawValue(), isActive: true }).subscribe({
      next: (created) => {
        this.customers.update((current) => [...current, created]);
        this.form.controls.customerId.setValue(created.id);
        this.customerDialogVisible.set(false);
      },
      error: (response: HttpErrorResponse) => this.customerError.set(this.describeError(response, 'Could not add customer.')),
    });
  }

  protected save(): void {
    const customerId = this.form.controls.customerId.value;
    if (customerId === null) {
      this.form.controls.customerId.markAsTouched();
      this.saveError.set('Choose a customer.');
      return;
    }

    const pricingName = this.form.controls.pricingName.value;
    if (!pricingName) {
      this.form.controls.pricingName.markAsTouched();
      this.saveError.set('Choose a pricing.');
      return;
    }

    const current = this.editing();
    if (current !== null && this.form.controls.status.value === 'Cancelled' && current.status !== 'Cancelled') {
      this.cancelStock.set(this.filamentOnCancel(current));
      this.cancelStockVisible.set(true);
      return;
    }

    this.persistOrder(customerId, pricingName);
  }

  protected consumedGrams(): number {
    return this.cancelStock().reduce((sum, row) => sum + row.used, 0);
  }

  protected confirmCancel(): void {
    const customerId = this.form.controls.customerId.value;
    const pricingName = this.form.controls.pricingName.value;
    if (customerId === null || !pricingName) {
      return;
    }
    this.cancelStockVisible.set(false);
    this.persistOrder(customerId, pricingName);
  }

  private filamentOnCancel(order: Order): { type: string; color: string; used: number; returned: number }[] {
    const rows = new Map<string, { type: string; color: string; used: number; returned: number }>();
    for (const line of order.items ?? []) {
      const key = `${line.filamentType}|${line.color}`;
      const current = rows.get(key) ?? { type: line.filamentType, color: line.color, used: 0, returned: 0 };
      const weight = line.weight ?? 0;
      current.used += weight * (line.completedQuantity ?? 0);
      current.returned += weight * (line.quantity - (line.completedQuantity ?? 0));
      rows.set(key, current);
    }
    return [...rows.values()];
  }

  private persistOrder(customerId: number, pricingName: string): void {
    const current = this.editing();
    const request = current === null
      ? this.ordersService.create(customerId, pricingName, [])
      : this.ordersService.update(
          current,
          customerId,
          this.form.controls.status.value,
          pricingName,
          current.items.map((line) => ({
            itemId: line.itemId,
            filamentType: line.filamentType,
            color: line.color,
            quantity: line.quantity,
            completedQuantity: line.completedQuantity ?? 0,
          })),
        );

    this.saving.set(true);
    request.subscribe({
      next: () => {
        this.saving.set(false);
        this.dialogVisible.set(false);
        this.load();
      },
      error: (response: HttpErrorResponse) => {
        this.saving.set(false);
        this.saveError.set(this.describeError(response, 'Could not save the order.'));
      },
    });
  }

  protected startAdd(order: Order): void {
    this.lineEdit.set(null);
    this.draftOrder.set(order.orderNumber);
    this.lineItemId.set(null);
    this.lineFilamentType.set(null);
    this.lineColor.set(null);
    this.lineQuantity.set(1);
    this.lineCompleted.set(0);
    this.lineError.set(null);
  }

  protected startLineEdit(order: Order, line: Order['items'][number]): void {
    this.draftOrder.set(null);
    this.lineEdit.set({ orderNumber: order.orderNumber, itemId: line.itemId, filamentType: line.filamentType, color: line.color });
    this.lineItemId.set(line.itemId);
    this.lineFilamentType.set(line.filamentType);
    const knownColor = this.filamentColors().some((color) => color.name === line.color);
    this.lineColor.set(knownColor ? line.color : null);
    this.lineQuantity.set(line.quantity);
    this.lineCompleted.set(line.completedQuantity ?? 0);
    this.lineError.set(null);
  }

  protected cancelLine(): void {
    this.lineEdit.set(null);
    this.draftOrder.set(null);
    this.lineError.set(null);
  }

  protected saveLine(order: Order): void {
    const itemId = this.lineItemId();
    const filamentType = this.lineFilamentType()?.trim() ?? '';
    const color = this.lineColor()?.trim() ?? '';
    const quantity = this.lineQuantity() || 1;
    const completedQuantity = Math.min(Math.max(this.lineCompleted() || 0, 0), quantity);
    if (itemId === null) {
      this.lineError.set('Select an item.');
      return;
    }
    if (filamentType === '' || color === '' || !this.filamentColors().some((entry) => entry.name === color)) {
      this.lineError.set('Choose a filament color from the list.');
      return;
    }

    const editing = this.lineEdit();
    const sameLine = (line: Order['items'][number]) =>
      line.itemId === itemId && line.filamentType === filamentType && line.color === color;
    const others = order.items.filter((line) =>
      editing === null
      || line.itemId !== editing.itemId
      || line.filamentType !== editing.filamentType
      || line.color !== editing.color);
    if (others.some(sameLine)) {
      this.lineError.set('This item is already on the order with that filament and color.');
      return;
    }

    const items = [
      ...others.map((line) => ({
        itemId: line.itemId,
        filamentType: line.filamentType,
        color: line.color,
        quantity: line.quantity,
        completedQuantity: line.completedQuantity ?? 0,
      })),
      { itemId, filamentType, color, quantity, completedQuantity },
    ];
    this.persistLines(order, items);
  }

  protected completeOne(order: Order, line: Order['items'][number]): void {
    const completed = Math.min((line.completedQuantity ?? 0) + 1, line.quantity);
    const items = order.items.map((current) => ({
      itemId: current.itemId,
      filamentType: current.filamentType,
      color: current.color,
      quantity: current.quantity,
      completedQuantity: current === line ? completed : (current.completedQuantity ?? 0),
    }));
    this.persistLines(order, items);
  }

  protected deleteLine(order: Order, line: Order['items'][number]): void {
    const items = order.items
      .filter((current) => current.itemId !== line.itemId || current.filamentType !== line.filamentType || current.color !== line.color)
      .map((current) => ({
        itemId: current.itemId,
        filamentType: current.filamentType,
        color: current.color,
        quantity: current.quantity,
        completedQuantity: current.completedQuantity ?? 0,
      }));
    this.persistLines(order, items);
  }

  private persistLines(order: Order, items: NewOrderLine[]): void {
    const customerId = this.customerIdFor(order);
    if (customerId === null) {
      this.lineError.set('Could not find the customer for this order.');
      return;
    }

    this.savingLine.set(true);
    this.ordersService.update(order, customerId, order.status ?? 'Pending', order.pricingName || 'Standard', items).subscribe({
      next: () => {
        this.savingLine.set(false);
        this.lineError.set(null);
        this.draftOrder.set(null);
        this.lineEdit.set(null);
        this.load();
      },
      error: (response: HttpErrorResponse) => {
        this.savingLine.set(false);
        this.lineError.set(this.describeError(response, 'Could not update the items.'));
      },
    });
  }

  protected orderedPieces(order: Order): number {
    return order.items.reduce((sum, line) => sum + line.quantity, 0);
  }

  protected completedPieces(order: Order): number {
    return order.items.reduce((sum, line) => sum + (line.completedQuantity ?? 0), 0);
  }

  protected completion(order: Order): number {
    const ordered = this.orderedPieces(order);
    return ordered === 0 ? 0 : Math.round((this.completedPieces(order) / ordered) * 100);
  }

  protected printingMinutesLeft(): number {
    return (this.orders() ?? [])
      .filter((order) => order.status === 'Pending' || order.status === 'Printing')
      .reduce((sum, order) => sum + this.remainingMinutes(order), 0);
  }

  protected printingDays(): number {
    return Math.floor(this.printingMinutesLeft() / 60 / 8);
  }

  protected printingRemainderMinutes(): number {
    return this.printingMinutesLeft() % (8 * 60);
  }

  protected nextStartDate(): Date {
    let hours = this.printingMinutesLeft() / 60;
    const date = new Date();
    date.setHours(0, 0, 0, 0);
    const nextWeekday = () => {
      date.setDate(date.getDate() + 1);
      while (date.getDay() === 0 || date.getDay() === 6) {
        date.setDate(date.getDate() + 1);
      }
    };
    while (date.getDay() === 0 || date.getDay() === 6) {
      date.setDate(date.getDate() + 1);
    }
    while (hours > 8) {
      hours -= 8;
      nextWeekday();
    }
    if (hours === 8) {
      nextWeekday();
    }
    return date;
  }

  protected statusBars(): { label: string; count: number; percent: number; color: string }[] {
    const orders = this.orders() ?? [];
    const total = orders.length;
    const groups = [
      { label: 'Pending', match: (status: string | null) => status === 'Pending', color: '#9ca3af' },
      { label: 'Printing', match: (status: string | null) => status === 'Printing', color: '#facc15' },
      { label: 'Shipped', match: (status: string | null) => status === 'Shipped', color: '#22c55e' },
      {
        label: 'Confirmed payment',
        match: (status: string | null) => status === 'Confirmed payment' || status === 'Delivered',
        color: '#38bdf8',
      },
      { label: 'Cancelled', match: (status: string | null) => status === 'Cancelled', color: '#ef4444' },
    ];
    return groups.map((group) => {
      const count = orders.filter((order) => group.match(order.status)).length;
      return {
        label: group.label,
        count,
        percent: total === 0 ? 0 : Math.round((count / total) * 100),
        color: group.color,
      };
    });
  }

  private remainingMinutes(order: Order): number {
    return (order.items ?? []).reduce((sum, line) => {
      const left = Math.max(0, line.quantity - (line.completedQuantity ?? 0));
      return sum + (line.printDurationMinutes ?? 0) * left;
    }, 0);
  }

  protected statusLabel(order: Order): string {
    if (order.status === 'Printing') {
      return `Printing ${this.completedPieces(order)}/${this.orderedPieces(order)}`;
    }
    return order.status ?? '';
  }

  protected pricingStyle(pricingName: string | null): Record<string, string> {
    const colors: Record<string, [string, string]> = {
      Standard: ['#22c55e', '#052e16'],
      Urgent: ['#ef4444', '#ffffff'],
    };
    const [background, color] = colors[pricingName ?? ''] ?? ['#e5e7eb', '#374151'];
    return {
      background,
      color,
      display: 'inline-block',
      padding: '0.2rem 0.65rem',
      'border-radius': '1rem',
      'font-weight': '600',
    };
  }

  protected statusStyle(status: string | null): Record<string, string> {
    const colors: Record<string, [string, string]> = {
      Pending: ['#e5e7eb', '#374151'],
      Printing: ['#facc15', '#713f12'],
      Shipped: ['#22c55e', '#052e16'],
      'Confirmed payment': ['#38bdf8', '#0c4a6e'],
      Cancelled: ['#ef4444', '#ffffff'],
    };
    const [background, color] = colors[status ?? ''] ?? colors['Pending'];
    return {
      background,
      color,
      display: 'inline-block',
      padding: '0.2rem 0.65rem',
      'border-radius': '1rem',
      'font-weight': '600',
    };
  }

  protected totalMinutes(order: Order): number {
    return order.items.reduce((sum, line) => sum + line.printDurationMinutes * line.quantity, 0);
  }

  protected totalPrice(order: Order): number {
    return order.items.reduce((sum, line) => sum + line.totalPrice, 0);
  }

  protected missingPrice(order: Order): boolean {
    return order.items.some((line) => line.pricePerUnit === null);
  }

  protected formatDuration(totalMinutes: number): string {
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;
    if (hours === 0) {
      return `${minutes}m`;
    }
    return minutes === 0 ? `${hours}h` : `${hours}h ${minutes}m`;
  }

  private customerIdFor(order: Order): number | null {
    if (order.customerId > 0) {
      return order.customerId;
    }
    return this.customers().find((customer) => customer.fullName === order.customerName)?.id ?? null;
  }

  private lineGroup(itemId: number | null = null, quantity = 1) {
    return this.fb.group({
      itemId: this.fb.control<number | null>(itemId, Validators.required),
      quantity: this.fb.nonNullable.control(quantity, [Validators.required, Validators.min(1)]),
    });
  }

  protected lowStock(): FilamentStock[] {
    return this.filamentStock()
      .filter((item) => item.quantity < 300)
      .sort((left, right) => left.quantity - right.quantity);
  }

  protected lowStockStyle(): Record<string, string> {
    return {
      background: '#ef4444',
      color: '#ffffff',
      display: 'inline-block',
      padding: '0.2rem 0.65rem',
      'border-radius': '1rem',
      'font-weight': '600',
    };
  }

  private loadStock(): void {
    this.filamentStockService.getAll().subscribe({
      next: (response) => this.filamentStock.set(response),
    });
  }

  private load(): void {
    this.ordersService.getAll().subscribe({
      next: (response) => {
        this.error.set(null);
        this.orders.set(response);
        this.loadStock();
      },
      error: () => this.error.set('Could not load orders.'),
    });
  }

  private describeError(response: HttpErrorResponse, fallback: string): string {
    const body = typeof response.error === 'string' ? this.readJson(response.error) : response.error;
    const messages = body?.errors ? Object.values(body.errors).flat() : [];
    const text = messages.filter((message) => typeof message === 'string').join(' ');
    return text || body?.detail || fallback;
  }

  private readJson(value: string): { errors?: Record<string, string[]>; detail?: string } | null {
    try {
      return JSON.parse(value);
    } catch {
      return null;
    }
  }
}
