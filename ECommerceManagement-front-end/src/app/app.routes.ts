import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { adminGuard } from './core/guards/admin-guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/pages/login/login')
        .then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/pages/register/register')
        .then(m => m.RegisterComponent)
  },
  {
    path: 'products',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/products/pages/product-list/product-list')
        .then(m => m.ProductListComponent)
  },
  {
    path: 'products/create',
    canActivate: [authGuard, adminGuard],
    loadComponent: () =>
        import('./features/products/pages/product-create/product-create')
        .then(m => m.ProductCreateComponent)
  },
  {
    path: 'products/edit/:id',
    canActivate: [authGuard, adminGuard],
    loadComponent: () =>
        import('./features/products/pages/product-edit/product-edit')
        .then(m => m.ProductEditComponent)
  }

];