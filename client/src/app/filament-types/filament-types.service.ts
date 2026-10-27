import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface FilamentType {
  id: number;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class FilamentTypesService {
  private readonly http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getAll(): Observable<FilamentType[]> {
    return this.http.get<FilamentType[]>(this.baseUrl + 'filament-types');
  }

  create(name: string): Observable<FilamentType> {
    return this.http.post<FilamentType>(this.baseUrl + 'filament-types', { name });
  }

  update(id: number, name: string): Observable<FilamentType> {
    return this.http.put<FilamentType>(this.baseUrl + 'filament-types/' + id, { name });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'filament-types/' + id);
  }
}
