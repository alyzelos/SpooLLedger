import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

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
  baseUrl = environment.apiUrl;

  getAll(): Observable<FilamentStock[]> {
    return this.http.get<FilamentStock[]>(this.baseUrl + 'stock');
  }
}
