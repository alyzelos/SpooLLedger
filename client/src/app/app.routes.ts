import { Routes } from '@angular/router';
import { FilamentStockPage } from './filament-stock/filament-stock';
import { Home } from './home/home';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'filament-stock', component: FilamentStockPage },
];
