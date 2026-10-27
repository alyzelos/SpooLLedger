import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface OrderLine {
  itemId: number;
  productName: string;
  filamentType: string;
  color: string;
  weight: number;
  printDurationMinutes: number;
  pricePerUnit: number | null;
  quantity: number;
  completedQuantity: number;
  totalWeight: number;
  totalPrice: number;
}

export interface Order {
  orderNumber: number;
  orderDate: string;
  status: string | null;
  pricingName: string;
  customerId: number;
  customerName: string;
  items: OrderLine[];
}

export interface NewOrderLine {
  itemId: number;
  filamentType: string;
  color: string;
  quantity: number;
  completedQuantity: number;
}

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private readonly http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getAll(): Observable<Order[]> {
    return this.http.get<Order[]>(this.baseUrl + 'orders');
  }

  create(customerId: number, pricingName: string, items: NewOrderLine[]): Observable<Order> {
    return this.http.post<Order>(this.baseUrl + 'orders/create-order', {
      orderDate: new Date().toISOString(),
      status: 'Pending',
      pricingName,
      customerId,
      items,
    });
  }

  update(order: Order, customerId: number, status: string, pricingName: string, items: NewOrderLine[]): Observable<Order> {
    return this.http.put<Order>(this.baseUrl + 'orders/update', {
      orderNumber: order.orderNumber,
      orderDate: order.orderDate,
      status,
      pricingName,
      customerId,
      items,
    });
  }

  delete(orderNumber: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'orders/' + orderNumber);
  }
}
