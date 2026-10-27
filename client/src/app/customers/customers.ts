import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { CheckboxModule } from 'primeng/checkbox';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { Customer, CustomerInput, CustomersService } from './customers.service';

@Component({
  selector: 'app-customers',
  imports: [
    ButtonModule,
    CardModule,
    CheckboxModule,
    ConfirmDialogModule,
    DatePipe,
    DialogModule,
    InputTextModule,
    ReactiveFormsModule,
    TableModule,
  ],
  providers: [ConfirmationService],
  templateUrl: './customers.html',
  styles: ':host { display: block; min-width: 0; }',
})
export class CustomersCard implements OnInit {
  private readonly customersService = inject(CustomersService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly fb = inject(FormBuilder);

  protected readonly customers = signal<Customer[] | null>(null);
  protected readonly error = signal<string | null>(null);
  protected readonly dialogVisible = signal(false);
  protected readonly editingId = signal<number | null>(null);
  protected readonly saving = signal(false);
  protected readonly saveError = signal<string | null>(null);
  protected readonly pageSizes = [5, 10, 15, 20];

  protected readonly form = this.fb.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
    phoneNumber: ['', [Validators.required, Validators.maxLength(30)]],
    isActive: [true],
  });

  ngOnInit(): void {
    this.load();
  }

  protected add(): void {
    this.editingId.set(null);
    this.form.reset({ isActive: true });
    this.openDialog();
  }

  protected edit(customer: Customer): void {
    this.editingId.set(customer.id);
    this.form.reset(customer);
    this.openDialog();
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const input: CustomerInput = this.form.getRawValue();
    const id = this.editingId();
    const request = id === null
      ? this.customersService.create(input)
      : this.customersService.update(id, input);

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

  protected delete(customer: Customer): void {
    this.confirmationService.confirm({
      header: 'Delete customer',
      message: `Delete ${customer.fullName}?`,
      icon: 'pi pi-trash',
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      accept: () =>
        this.customersService.delete(customer.id).subscribe({
          next: () => this.load(),
          error: (response: HttpErrorResponse) =>
            this.error.set(
              response.status === 409
                ? `${customer.fullName} has orders and cannot be deleted.`
                : 'Could not delete customer.',
            ),
        }),
    });
  }

  protected invalid(name: 'firstName' | 'lastName' | 'email' | 'phoneNumber'): boolean {
    const control = this.form.controls[name];
    return control.invalid && control.touched;
  }

  private openDialog(): void {
    this.saveError.set(null);
    this.dialogVisible.set(true);
  }

  private load(): void {
    this.customersService.getAll().subscribe({
      next: (response) => {
        this.error.set(null);
        this.customers.set(response);
      },
      error: () => this.error.set('Could not load customers.'),
    });
  }

  private describeError(response: HttpErrorResponse): string {
    if (response.status === 400 && response.error?.errors) {
      return Object.values<string[]>(response.error.errors).flat().join(' ');
    }
    if (response.status === 404) {
      return 'This customer no longer exists.';
    }
    return 'Could not save customer.';
  }
}
