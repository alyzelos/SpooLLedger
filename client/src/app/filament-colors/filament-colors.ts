import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { FilamentColor, FilamentColorsService } from './filament-colors.service';

@Component({
  selector: 'app-filament-colors',
  imports: [
    ButtonModule,
    CardModule,
    ConfirmDialogModule,
    DialogModule,
    InputTextModule,
    ReactiveFormsModule,
    TableModule,
  ],
  providers: [ConfirmationService],
  templateUrl: './filament-colors.html',
  styles: ':host { display: block; min-width: 0; }',
})
export class FilamentColorsCard implements OnInit {
  private readonly filamentColorsService = inject(FilamentColorsService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly fb = inject(FormBuilder);

  protected readonly colors = signal<FilamentColor[] | null>(null);
  protected readonly error = signal<string | null>(null);
  protected readonly dialogVisible = signal(false);
  protected readonly editingId = signal<number | null>(null);
  protected readonly saving = signal(false);
  protected readonly saveError = signal<string | null>(null);
  protected readonly pageSizes = [5, 10, 15, 20];

  protected readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
  });

  ngOnInit(): void {
    this.load();
  }

  protected add(): void {
    this.editingId.set(null);
    this.form.reset();
    this.openDialog();
  }

  protected edit(color: FilamentColor): void {
    this.editingId.set(color.id);
    this.form.reset({ name: color.name });
    this.openDialog();
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const name = this.form.getRawValue().name.trim();
    const id = this.editingId();
    const request = id === null
      ? this.filamentColorsService.create(name)
      : this.filamentColorsService.update(id, name);

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

  protected delete(color: FilamentColor): void {
    this.confirmationService.confirm({
      header: 'Delete filament color',
      message: `Delete ${color.name}?`,
      icon: 'pi pi-trash',
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      accept: () =>
        this.filamentColorsService.delete(color.id).subscribe({
          next: () => this.load(),
          error: (response: HttpErrorResponse) =>
            this.error.set(
              response.status === 409
                ? `${color.name} is used by filament stock and cannot be deleted.`
                : 'Could not delete filament color.',
            ),
        }),
    });
  }

  protected invalid(): boolean {
    const control = this.form.controls.name;
    return control.invalid && control.touched;
  }

  private openDialog(): void {
    this.saveError.set(null);
    this.dialogVisible.set(true);
  }

  private load(): void {
    this.filamentColorsService.getAll().subscribe({
      next: (response) => {
        this.error.set(null);
        this.colors.set(response);
      },
      error: () => this.error.set('Could not load filament colors.'),
    });
  }

  private describeError(response: HttpErrorResponse): string {
    if (response.status === 400 && response.error?.errors) {
      return Object.values<string[]>(response.error.errors).flat().join(' ');
    }
    if (response.status === 404) {
      return 'This filament color no longer exists.';
    }
    return 'Could not save filament color.';
  }
}
