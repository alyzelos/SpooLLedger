import { Component } from '@angular/core';
import { FilamentColorsCard } from '../filament-colors/filament-colors';
import { FilamentStockPage } from '../filament-stock/filament-stock';
import { FilamentTypesCard } from '../filament-types/filament-types';

@Component({
  selector: 'app-resources',
  imports: [FilamentColorsCard, FilamentStockPage, FilamentTypesCard],
  templateUrl: './resources.html',
})
export class ResourcesPage {}
