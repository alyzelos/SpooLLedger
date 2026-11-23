import { Routes } from '@angular/router';
import { Home } from './home/home';
import { OrdersPage } from './orders/orders';
import { ItemsPage } from './items/items';
import { PricingPage } from './pricing/pricing';
import { CustomersCard } from './customers/customers';
import { ResourcesPage } from './resources/resources';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'orders', component: OrdersPage },
  { path: 'pricing', component: PricingPage },
  { path: 'resources', component: ResourcesPage },
  { path: 'items', component: ItemsPage },
  { path: 'customers', component: CustomersCard },
];
