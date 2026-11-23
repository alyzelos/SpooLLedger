import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface FilamentColor {
  id: number;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class FilamentColorsService {
  private readonly http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getAll(): Observable<FilamentColor[]> {
    return this.http.get<FilamentColor[]>(this.baseUrl + 'filament-colors');
  }

  create(name: string): Observable<FilamentColor> {
    return this.http.post<FilamentColor>(this.baseUrl + 'filament-colors', { name });
  }

  update(id: number, name: string): Observable<FilamentColor> {
    return this.http.put<FilamentColor>(this.baseUrl + 'filament-colors/' + id, { name });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'filament-colors/' + id);
  }
}
