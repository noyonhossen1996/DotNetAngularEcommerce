import { ProductImage } from './product-image.model';

export interface Product {
  id: string;
  name: string;
  description?: string;
  sku: string;
  price: number;
  isActive: boolean;
  categoryId: string;
  categoryName: string;
  brandId: string;
  brandName: string;
  images: ProductImage[];
}