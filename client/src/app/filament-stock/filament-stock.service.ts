import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface FilamentStock {
  id: number;
  producer: string | null;
  type: string | null;
  color: string | null;
  quantity: number;
}

@Injectable({ providedIn: 'root' })
export class FilamentStockService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'https://localhost:5101/api/stock';

  getAll(): Observable<FilamentStock[]> {
    return this.http.get<FilamentStock[]>(this.baseUrl);
  }
}
