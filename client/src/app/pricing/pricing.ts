import { Component, inject, OnInit, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { Pricing, PricingInput, PricingService } from './pricing.service';

const gramsPerSpool = 1000;

@Component({
  selector: 'app-pricing',
  imports: [
    ButtonModule,
    CardModule,
    ConfirmDialogModule,
    CurrencyPipe,
    DialogModule,
    InputNumberModule,
    InputTextModule,
    ReactiveFormsModule,
    TableModule,
  ],
  providers: [ConfirmationService],
  templateUrl: './pricing.html',
  styleUrl: './pricing.css',
})
export class PricingPage implements OnInit {
  private readonly pricingService = inject(PricingService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly fb = inject(FormBuilder);

  protected readonly pricings = signal<Pricing[] | null>(null);
  protected readonly error = signal<string | null>(null);
  protected readonly dialogVisible = signal(false);
  protected readonly editingId = signal<number | null>(null);
  protected readonly saving = signal(false);
  protected readonly saveError = signal<string | null>(null);

  protected readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    electricityCostPerKwh: [0, [Validators.required, Validators.min(0)]],
    filamentSpoolPrice: [0, [Validators.required, Validators.min(0)]],
    printerMaintenanceCostPerHour: [0, [Validators.required, Validators.min(0)]],
  });

  ngOnInit(): void {
    this.load();
  }

  protected pricePerGram(): number {
    return (this.form.controls.filamentSpoolPrice.value ?? 0) / gramsPerSpool;
  }

  protected add(): void {
    this.editingId.set(null);
    this.form.reset();
    this.openDialog();
  }

  protected edit(item: Pricing): void {
    this.editingId.set(item.id);
    this.form.reset({
      name: item.name,
      electricityCostPerKwh: item.electricityCostPerKwh,
      filamentSpoolPrice: item.filamentSpoolPrice,
      printerMaintenanceCostPerHour: item.printerMaintenanceCostPerHour,
    });
    this.openDialog();
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const input: PricingInput = this.form.getRawValue();
    const id = this.editingId();
    const request = id === null
      ? this.pricingService.create(input)
      : this.pricingService.update(id, input);

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

  protected delete(item: Pricing): void {
    this.confirmationService.confirm({
      header: 'Delete pricing',
      message: `Delete pricing "${item.name}"?`,
      icon: 'pi pi-trash',
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      accept: () =>
        this.pricingService.delete(item.id).subscribe({
          next: () => this.load(),
          error: () => this.error.set('Could not delete pricing.'),
        }),
    });
  }

  protected invalid(name: keyof PricingInput): boolean {
    const control = this.form.controls[name];
    return control.invalid && control.touched;
  }

  private openDialog(): void {
    this.saveError.set(null);
    this.dialogVisible.set(true);
  }

  private load(): void {
    this.pricingService.getAll().subscribe({
      next: (response) => {
        this.error.set(null);
        this.pricings.set(response);
      },
      error: () => this.error.set('Could not load pricing.'),
    });
  }

  private describeError(response: HttpErrorResponse): string {
    if (response.status === 400 && response.error?.errors) {
      return Object.values<string[]>(response.error.errors).flat().join(' ');
    }
    if (response.status === 404) {
      return 'This pricing no longer exists.';
    }
    return 'Could not save pricing.';
  }
}
