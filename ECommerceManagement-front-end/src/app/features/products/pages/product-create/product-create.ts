import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { CategoryService } from '../../../categories/services/category';
import { BrandService } from '../../../brands/services/brand';
import { Category } from '../../../../shared/models/category';
import { Brand } from '../../../../shared/models/brand';

@Component({
  selector: 'app-product-create',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './product-create.html',
  styleUrl: './product-create.css'
})
export class ProductCreateComponent {
  private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private brandService = inject(BrandService);
  private router = inject(Router);

  categories = signal<Category[]>([]);
  brands = signal<Brand[]>([]);
  loading = signal(false);
  error = signal('');

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    description: [''],
    sku: ['', [Validators.required, Validators.maxLength(100)]],
    price: [0, [Validators.required, Validators.min(1)]],
    categoryId: ['', Validators.required],
    brandId: ['', Validators.required]
  });

  constructor() {
    this.loadDropdowns();
  }

  loadDropdowns(): void {
    this.categoryService.getAll().subscribe({
      next: data => this.categories.set(data)
    });

    this.brandService.getAll().subscribe({
      next: data => this.brands.set(data)
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.error.set('');

    this.productService.create(this.form.getRawValue() as any).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/products']);
      },
      error: err => {
        this.loading.set(false);
        this.error.set(err?.error?.message ?? 'Product create failed.');
      }
    });
  }
}
