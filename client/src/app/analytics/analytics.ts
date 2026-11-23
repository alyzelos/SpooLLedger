import { Component, inject, OnInit, signal } from '@angular/core';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { Order, OrdersService } from '../orders/orders.service';

interface RankedItem {
  name: string;
  pieces: number;
}

@Component({
  selector: 'app-analytics',
  imports: [CardModule, TableModule],
  templateUrl: './analytics.html',
})
export class AnalyticsPage implements OnInit {
  private readonly ordersService = inject(OrdersService);

  protected readonly items = signal<RankedItem[] | null>(null);
  protected readonly colors = signal<RankedItem[] | null>(null);
  protected readonly error = signal<string | null>(null);
  protected readonly pageSizes = [10, 20, 30];

  ngOnInit(): void {
    this.ordersService.getAll().subscribe({
      next: (orders) => {
        const counted = orders.filter((order) => order.status !== 'Cancelled');
        this.items.set(this.rank(counted, (line) => line.productName || 'Item'));
        this.colors.set(this.rank(counted, (line) => line.color || 'Unknown'));
        this.error.set(null);
      },
      error: () => this.error.set('Could not load analytics.'),
    });
  }

  private rank(orders: Order[], nameOf: (line: Order['items'][number]) => string): RankedItem[] {
    const totals = new Map<string, number>();
    for (const order of orders) {
      for (const line of order.items ?? []) {
        const name = nameOf(line);
        totals.set(name, (totals.get(name) ?? 0) + (line.quantity ?? 0));
      }
    }
    return [...totals.entries()]
      .map(([name, pieces]) => ({ name, pieces }))
      .sort((a, b) => b.pieces - a.pieces || a.name.localeCompare(b.name));
  }
}
