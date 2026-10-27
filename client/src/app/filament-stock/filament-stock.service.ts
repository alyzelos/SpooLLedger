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

export type FilamentStockInput = Omit<FilamentStock, 'id'>;

export interface FilamentType {
  id: number;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class FilamentStockService {
  private readonly http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getAll(): Observable<FilamentStock[]> {
    return this.http.get<FilamentStock[]>(this.baseUrl + 'stock');
  }

  getTypes(): Observable<FilamentType[]> {
    return this.http.get<FilamentType[]>(this.baseUrl + 'filament-types');
  }

  getColors(): Observable<FilamentType[]> {
    return this.http.get<FilamentType[]>(this.baseUrl + 'filament-colors');
  }

  create(input: FilamentStockInput): Observable<FilamentStock> {
    return this.http.post<FilamentStock>(this.baseUrl + 'stock', input);
  }

  update(id: number, input: FilamentStockInput): Observable<FilamentStock> {
    return this.http.put<FilamentStock>(this.baseUrl + 'stock/' + id, input);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'stock/' + id);
  }
}
