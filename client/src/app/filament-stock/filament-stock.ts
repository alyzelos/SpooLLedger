import { NgStyle } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';
import {
  FilamentStock,
  FilamentStockInput,
  FilamentStockService,
  FilamentType,
} from './filament-stock.service';

@Component({
  selector: 'app-filament-stock',
  imports: [
    ButtonModule,
    CardModule,
    ConfirmDialogModule,
    DialogModule,
    NgStyle,
    InputNumberModule,
    InputTextModule,
    ReactiveFormsModule,
    SelectModule,
    TableModule,
  ],
  providers: [ConfirmationService],
  templateUrl: './filament-stock.html',
  styles: ':host { display: block; min-width: 0; }',
})
export class FilamentStockPage implements OnInit {
  private readonly filamentStockService = inject(FilamentStockService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly fb = inject(FormBuilder);

  protected readonly pageSizes = [5, 10, 15, 20];
  protected readonly stock = signal<FilamentStock[] | null>(null);
  protected readonly error = signal<string | null>(null);
  protected readonly types = signal<FilamentType[]>([]);
  protected readonly colors = signal<FilamentType[]>([]);
  protected readonly dialogVisible = signal(false);
  protected readonly editingId = signal<number | null>(null);
  protected readonly saving = signal(false);
  protected readonly saveError = signal<string | null>(null);

  protected readonly form = this.fb.nonNullable.group({
    producer: ['', [Validators.required, Validators.maxLength(100)]],
    type: ['', [Validators.required, Validators.maxLength(100)]],
    color: ['', [Validators.required, Validators.maxLength(100)]],
    quantity: [0, [Validators.required, Validators.min(0)]],
  });

  ngOnInit(): void {
    this.load();
    this.loadTypes();
    this.loadColors();
  }

  protected add(): void {
    this.editingId.set(null);
    this.form.reset();
    this.openDialog();
  }

  protected edit(item: FilamentStock): void {
    this.editingId.set(item.id);
    this.form.reset({
      producer: item.producer ?? '',
      type: item.type ?? '',
      color: item.color ?? '',
      quantity: item.quantity,
    });
    this.openDialog();
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const input: FilamentStockInput = this.form.getRawValue();
    const id = this.editingId();
    const request = id === null
      ? this.filamentStockService.create(input)
      : this.filamentStockService.update(id, input);

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

  protected delete(item: FilamentStock): void {
    this.confirmationService.confirm({
      header: 'Delete filament',
      message: `Delete ${item.producer} ${item.type} ${item.color}?`,
      icon: 'pi pi-trash',
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      accept: () =>
        this.filamentStockService.delete(item.id).subscribe({
          next: () => this.load(),
          error: () => this.error.set('Could not delete filament.'),
        }),
    });
  }

  protected quantityStyle(quantity: number): Record<string, string> {
    const [background, color] = quantity < 300
      ? ['#ef4444', '#ffffff']
      : quantity <= 500
        ? ['#facc15', '#713f12']
        : ['#22c55e', '#052e16'];
    return {
      background,
      color,
      display: 'inline-block',
      padding: '0.2rem 0.65rem',
      'border-radius': '1rem',
      'font-weight': '600',
    };
  }

  protected invalid(name: keyof FilamentStockInput): boolean {
    const control = this.form.controls[name];
    return control.invalid && control.touched;
  }

  private openDialog(): void {
    this.saveError.set(null);
    if (this.types().length === 0) {
      this.loadTypes();
    }
    if (this.colors().length === 0) {
      this.loadColors();
    }
    this.dialogVisible.set(true);
  }

  private loadTypes(): void {
    this.filamentStockService.getTypes().subscribe({
      next: (response) => this.types.set(response),
      error: () => this.saveError.set('Could not load filament types.'),
    });
  }

  private loadColors(): void {
    this.filamentStockService.getColors().subscribe({
      next: (response) => this.colors.set(response),
      error: () => this.saveError.set('Could not load filament colors.'),
    });
  }

  private load(): void {
    this.filamentStockService.getAll().subscribe({
      next: (response) => {
        this.error.set(null);
        this.stock.set(response);
      },
      error: () => this.error.set('Could not load filament stock.'),
    });
  }

  private describeError(response: HttpErrorResponse): string {
    if (response.status === 400 && response.error?.errors) {
      return Object.values<string[]>(response.error.errors).flat().join(' ');
    }
    if (response.status === 404) {
      return 'This filament no longer exists.';
    }
    return 'Could not save filament.';
  }
}
