import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { CategoryService } from '../../../categories/services/category';
import { BrandService } from '../../../brands/services/brand';
import { Category } from '../../../../shared/models/category';
import { Brand } from '../../../../shared/models/brand';

@Component({
  selector: 'app-product-edit',
  imports: [ReactiveFormsModule],
  templateUrl: './product-edit.html',
  styleUrl: './product-edit.css'
})
export class ProductEditComponent {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private brandService = inject(BrandService);

  productId = this.route.snapshot.paramMap.get('id')!;
  categories = signal<Category[]>([]);
  brands = signal<Brand[]>([]);
  loading = signal(false);
  error = signal('');
  productImages = signal<any[]>([]);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    description: [''],
    sku: ['', [Validators.required, Validators.maxLength(100)]],
    price: [0, [Validators.required, Validators.min(1)]],
    categoryId: ['', Validators.required],
    brandId: ['', Validators.required],
    isActive: [true]
  });

  constructor() {
    this.loadDropdowns();
    this.loadProduct();
  }

  loadDropdowns(): void {
    this.categoryService.getAll().subscribe({
      next: data => this.categories.set(data)
    });

    this.brandService.getAll().subscribe({
      next: data => this.brands.set(data)
    });
  }

  loadProduct(): void {
    this.productService.getById(this.productId).subscribe({
      next: product => {
        this.form.patchValue({
          name: product.name,
          description: product.description ?? '',
          sku: product.sku,
          price: product.price,
          categoryId: product.categoryId,
          brandId: product.brandId,
          isActive: product.isActive
        });
        this.productImages.set(product.images ?? []);
      },
      error: () => this.error.set('Product not found.')
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    this.productService.update(this.productId, this.form.getRawValue() as any).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/products']);
      },
      error: err => {
        this.loading.set(false);
        this.error.set(err?.error?.message ?? 'Product update failed.');
      }
    });
  }

  selectedFile: File | null = null;

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  uploadImage(): void {
    if (!this.selectedFile) {
      this.error.set('Please select an image.');
      return;
    }

    this.productService.uploadImage(this.productId, this.selectedFile).subscribe({
      next: () => {
        this.selectedFile = null;
        this.loadProduct();
      },
      error: err => {
        this.error.set(err?.error?.message ?? 'Image upload failed.');
      }
    });
  }
}