import { Component, inject, OnInit, signal } from '@angular/core';
import { CardModule } from 'primeng/card';
import { FilamentStock, FilamentStockService } from './filament-stock.service';

@Component({
  selector: 'app-filament-stock',
  imports: [CardModule],
  templateUrl: './filament-stock.html',
  styleUrl: './filament-stock.css',
})
export class FilamentStockPage implements OnInit {
  private readonly filamentStockService = inject(FilamentStockService);
  protected readonly stock = signal<FilamentStock[] | null>(null);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.filamentStockService.getAll().subscribe({
      next: (response) => this.stock.set(response),
      error: () => this.error.set('Could not load filament stock.'),
    });
  }
}
