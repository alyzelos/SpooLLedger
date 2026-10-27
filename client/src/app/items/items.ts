import { Component, inject, OnInit, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';
import { TextareaModule } from 'primeng/textarea';
import { TooltipModule } from 'primeng/tooltip';
import { CustomersCard } from '../customers/customers';
import { FilamentTypesCard } from '../filament-types/filament-types';
import { Item, ItemInput, ItemsService } from './items.service';
import { Pricing, PricingService } from '../pricing/pricing.service';

@Component({
  selector: 'app-items',
  imports: [
    ButtonModule,
    CardModule,
    ConfirmDialogModule,
    CustomersCard,
    FilamentTypesCard,
    CurrencyPipe,
    DialogModule,
    FormsModule,
    InputNumberModule,
    InputTextModule,
    ReactiveFormsModule,
    SelectModule,
    TableModule,
    TextareaModule,
    TooltipModule,
  ],
  providers: [ConfirmationService],
  templateUrl: './items.html',
  styleUrl: './items.css',
})
export class ItemsPage implements OnInit {
  private readonly itemsService = inject(ItemsService);
  private readonly pricingService = inject(PricingService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly fb = inject(FormBuilder);

  protected readonly items = signal<Item[] | null>(null);
  protected readonly error = signal<string | null>(null);
  protected readonly dialogVisible = signal(false);
  protected readonly editingId = signal<number | null>(null);
  protected readonly saving = signal(false);
  protected readonly saveError = signal<string | null>(null);
  protected readonly priceItem = signal<Item | null>(null);
  protected readonly pricings = signal<Pricing[]>([]);
  protected readonly pricingName = signal<string | null>(null);
  protected readonly calculating = signal(false);
  protected readonly priceError = signal<string | null>(null);

  protected readonly form = this.fb.nonNullable.group({
    productName: ['', [Validators.required, Validators.maxLength(100)]],
    weight: [0, [Validators.required, Validators.min(1)]],
    hours: [0, [Validators.required, Validators.min(0)]],
    minutes: [0, [Validators.required, Validators.min(0), Validators.max(59)]],
    details: ['', [Validators.maxLength(1000)]],
  });

  ngOnInit(): void {
    this.load();
  }

  protected formatDuration(totalMinutes: number): string {
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;
    if (hours === 0) {
      return `${minutes}m`;
    }
    return minutes === 0 ? `${hours}h` : `${hours}h ${minutes}m`;
  }

  protected durationMissing(): boolean {
    const { hours, minutes } = this.form.getRawValue();
    return (hours ?? 0) * 60 + (minutes ?? 0) <= 0
      && (this.form.controls.hours.touched || this.form.controls.minutes.touched);
  }

  protected add(): void {
    this.editingId.set(null);
    this.form.reset();
    this.openDialog();
  }

  protected edit(item: Item): void {
    this.editingId.set(item.id);
    this.form.reset({
      productName: item.productName ?? '',
      weight: item.weight,
      hours: Math.floor(item.printDurationMinutes / 60),
      minutes: item.printDurationMinutes % 60,
      details: item.details ?? '',
    });
    this.openDialog();
  }

  protected calculatePrice(item: Item): void {
    this.priceItem.set(item);
    this.pricingName.set(this.pricings()[0]?.name ?? null);
    this.priceError.set(null);
    if (this.pricings().length === 0) {
      this.pricingService.getAll().subscribe({
        next: (response) => {
          this.pricings.set(response);
          this.pricingName.set(response[0]?.name ?? null);
        },
        error: () => this.priceError.set('Could not load pricing.'),
      });
    }
  }

  protected confirmPrice(): void {
    const item = this.priceItem();
    const pricingName = this.pricingName();
    if (item === null || pricingName === null) {
      return;
    }

    this.calculating.set(true);
    this.itemsService.calculatePrice(item.id, pricingName).subscribe({
      next: () => {
        this.calculating.set(false);
        this.priceItem.set(null);
        this.load();
      },
      error: (response: HttpErrorResponse) => {
        this.calculating.set(false);
        this.priceError.set(response.status === 400 ? 'Choose a pricing.' : 'Could not calculate the price.');
      },
    });
  }

  protected save(): void {
    this.form.markAllAsTouched();
    const { productName, weight, hours, minutes, details } = this.form.getRawValue();
    const printDurationMinutes = (hours ?? 0) * 60 + (minutes ?? 0);
    if (this.form.invalid || printDurationMinutes <= 0) {
      return;
    }

    const input: ItemInput = {
      productName,
      weight,
      printDurationMinutes,
      details: details.trim() === '' ? null : details,
    };
    const id = this.editingId();
    const request = id === null
      ? this.itemsService.create(input)
      : this.itemsService.update(id, input);

    this.saving.set(true);
    request.subscribe({
      next: () => {
        this.saving.set(false);
        this.dialogVisible.set(false);
        this.load();
      },
      error: (response: HttpErrorResponse) => {
        this.saving.set(false);
        this.saveError.set(this.describeError(response));
      },
    });
  }

  protected delete(item: Item): void {
    this.confirmationService.confirm({
      header: 'Delete item',
      message: `Delete "${item.productName}"?`,
      icon: 'pi pi-trash',
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      accept: () =>
        this.itemsService.delete(item.id).subscribe({
          next: () => this.load(),
          error: (response: HttpErrorResponse) =>
            this.error.set(
              response.status === 409
                ? `"${item.productName}" is used in orders and cannot be deleted.`
                : 'Could not delete item.',
            ),
        }),
    });
  }

  protected invalid(name: 'productName' | 'weight' | 'details'): boolean {
    const control = this.form.controls[name];
    return control.invalid && control.touched;
  }

  private openDialog(): void {
    this.saveError.set(null);
    this.dialogVisible.set(true);
  }

  private load(): void {
    this.itemsService.getAll().subscribe({
      next: (response) => {
        this.error.set(null);
        this.items.set(response);
      },
      error: () => this.error.set('Could not load items.'),
    });
  }

  private describeError(response: HttpErrorResponse): string {
    if (response.status === 400 && response.error?.errors) {
      return Object.values<string[]>(response.error.errors).flat().join(' ');
    }
    if (response.status === 404) {
      return 'This item no longer exists.';
    }
    return 'Could not save item.';
  }
}
