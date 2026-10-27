import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Pricing {
  id: number;
  name: string;
  electricityCostPerKwh: number;
  filamentSpoolPrice: number;
  filamentCostPerGram: number;
  printerMaintenanceCostPerHour: number;
}

export type PricingInput = Omit<Pricing, 'id' | 'filamentCostPerGram'>;

@Injectable({ providedIn: 'root' })
export class PricingService {
  private readonly http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getAll(): Observable<Pricing[]> {
    return this.http.get<Pricing[]>(this.baseUrl + 'pricing');
  }

  create(input: PricingInput): Observable<Pricing> {
    return this.http.post<Pricing>(this.baseUrl + 'pricing', input);
  }

  update(id: number, input: PricingInput): Observable<Pricing> {
    return this.http.put<Pricing>(this.baseUrl + 'pricing/' + id, input);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'pricing/' + id);
  }
}
