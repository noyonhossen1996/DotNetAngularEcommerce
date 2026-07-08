import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product.service';
import { Product } from '../../../../shared/models/product.model';
import { finalize } from 'rxjs';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../auth/services/auth';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css'
})
export class ProductListComponent {

  private productService = inject(ProductService);

  products = signal<Product[]>([]);
  loading = signal(false);
  error = signal('');
  authService = inject(AuthService);
  
  constructor() {
    this.loadProducts();
  }

  loadProducts() {

    this.loading.set(true);

    this.productService
      .getAll()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({

        next: products => {
          this.products.set(products);
        },

        error: () => {
          this.error.set('Failed to load products');
        }

      });

  }

  deleteProduct(id: string): void {
    const confirmed = confirm('Are you sure you want to delete this product?');

    if (!confirmed) return;

    this.productService.delete(id).subscribe({
      next: () => {
        this.products.update(items => items.filter(x => x.id !== id));
      },
      error: () => {
        this.error.set('Failed to delete product.');
      }
    });
  }

}