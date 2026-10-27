import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phoneNumber: string;
  isActive: boolean;
  createdAt: string;
}

export type CustomerInput = Pick<Customer, 'firstName' | 'lastName' | 'email' | 'phoneNumber' | 'isActive'>;

@Injectable({ providedIn: 'root' })
export class CustomersService {
  private readonly http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getAll(): Observable<Customer[]> {
    return this.http.get<Customer[]>(this.baseUrl + 'customers');
  }

  create(input: CustomerInput): Observable<Customer> {
    return this.http.post<Customer>(this.baseUrl + 'customers', input);
  }

  update(id: number, input: CustomerInput): Observable<Customer> {
    return this.http.put<Customer>(this.baseUrl + 'customers/' + id, input);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'customers/' + id);
  }
}
