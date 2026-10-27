import { Routes } from '@angular/router';
import { FilamentStockPage } from './filament-stock/filament-stock';
import { Home } from './home/home';
import { ItemsPage } from './items/items';
import { PricingPage } from './pricing/pricing';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'filament-stock', component: FilamentStockPage },
  { path: 'pricing', component: PricingPage },
  { path: 'resources', component: ItemsPage },
];
