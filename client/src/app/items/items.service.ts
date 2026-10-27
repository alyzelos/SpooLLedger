import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Item {
  id: number;
  productName: string | null;
  weight: number;
  printDurationMinutes: number;
  details: string | null;
  pricePerUnit: number | null;
}

export type ItemInput = Omit<Item, 'id' | 'pricePerUnit'>;

@Injectable({ providedIn: 'root' })
export class ItemsService {
  private readonly http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getAll(): Observable<Item[]> {
    return this.http.get<Item[]>(this.baseUrl + 'items');
  }

  create(input: ItemInput): Observable<Item> {
    return this.http.post<Item>(this.baseUrl + 'items', input);
  }

  update(id: number, input: ItemInput): Observable<Item> {
    return this.http.put<Item>(this.baseUrl + 'items/' + id, input);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'items/' + id);
  }
}
