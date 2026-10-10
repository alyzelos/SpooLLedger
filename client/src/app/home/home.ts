import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { CardModule } from 'primeng/card';

export interface FilamentStock {
  id: number;
  producer: string | null;
  type: string | null;
  color: string | null;
  quantity: number;
}

@Component({
  selector: 'app-home',
  imports: [CardModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private readonly http = inject(HttpClient);
  protected readonly stock = signal<FilamentStock[] | null>(null);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.http.get<FilamentStock[]>('https://localhost:5101/api/stock').subscribe({
      next: (response) => this.stock.set(response),
      error: () => this.error.set('Could not load filament stock.'),
    });
  }
}
