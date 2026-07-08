import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../shared/models/api-response.model';
import { Product } from '../../../shared/models/product.model';
import { ProductCreate } from '../../../shared/models/product-create';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private http = inject(HttpClient);

  private apiUrl = `${environment.apiBaseUrl}/products`;

  getAll(): Observable<Product[]> {

    return this.http
      .get<ApiResponse<Product[]>>(this.apiUrl)
      .pipe(map(response => response.data));

  }

  create(request: ProductCreate): Observable<Product> {
    return this.http
        .post<ApiResponse<Product>>(this.apiUrl, request)
        .pipe(map(response => response.data));
  }

  delete(id: string): Observable<string> {
    return this.http
        .delete<ApiResponse<string>>(`${this.apiUrl}/${id}`)
        .pipe(map(response => response.data));
  }

    getById(id: string): Observable<Product> {
    return this.http
        .get<ApiResponse<Product>>(`${this.apiUrl}/${id}`)
        .pipe(map(response => response.data));
    }

    update(id: string, request: ProductCreate): Observable<string> {
    return this.http
        .put<ApiResponse<string>>(`${this.apiUrl}/${id}`, request)
        .pipe(map(response => response.data));
    }

    uploadImage(productId: string, file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);

        return this.http
            .post<ApiResponse<any>>(`${this.apiUrl}/${productId}/images`, formData)
            .pipe(map(response => response.data));
    }

}