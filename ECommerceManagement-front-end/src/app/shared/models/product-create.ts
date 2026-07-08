export interface ProductCreate {
  name: string;
  description?: string;
  sku: string;
  price: number;
  categoryId: string;
  brandId: string;
}