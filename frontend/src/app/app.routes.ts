import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { CatalogComponent } from './components/catalog/catalog.component';
import { CartComponent } from './components/cart/cart.component';
import { CustomerOrdersComponent } from './components/customer-orders/customer-orders.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { AdminMedicinesComponent } from './components/admin-medicines/admin-medicines.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/catalog', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'catalog', component: CatalogComponent },
  { path: 'cart', component: CartComponent },
  { path: 'my-orders', component: CustomerOrdersComponent, canActivate: [authGuard], data: { role: 'Customer' } },
  { path: 'admin/dashboard', component: AdminDashboardComponent, canActivate: [authGuard], data: { role: 'Admin' } },
  { path: 'admin/medicines', component: AdminMedicinesComponent, canActivate: [authGuard], data: { role: 'Admin' } },
  { path: '**', redirectTo: '/catalog' }
];
