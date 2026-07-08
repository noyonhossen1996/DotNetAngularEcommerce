import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../shared/models/api-response.model';
import { Brand } from '../../../shared/models/brand';

@Injectable({
  providedIn: 'root'
})
export class BrandService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiBaseUrl}/brands`;

  getAll(): Observable<Brand[]> {
    return this.http
      .get<ApiResponse<Brand[]>>(this.apiUrl)
      .pipe(map(response => response.data));
  }
}