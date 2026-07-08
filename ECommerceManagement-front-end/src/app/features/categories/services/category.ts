import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../shared/models/api-response.model';
import { Category } from '../../../shared/models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiBaseUrl}/categories`;

  getAll(): Observable<Category[]> {
    return this.http
      .get<ApiResponse<Category[]>>(this.apiUrl)
      .pipe(map(response => response.data));
  }
}