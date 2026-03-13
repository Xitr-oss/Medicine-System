import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CartService, CartItem } from '../../services/cart.service';
import { OrderService } from '../../services/order.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent {
  cartService = inject(CartService);
  orderService = inject(OrderService);
  authService = inject(AuthService);
  router = inject(Router);

  cartItems: CartItem[] = [];
  total = 0;
  notes = '';
  loading = false;
  error = '';
  success = '';

  constructor() {
    this.cartService.cart$.subscribe(items => {
      this.cartItems = items;
      this.total = this.cartService.getTotal();
    });
  }

  updateQuantity(item: CartItem, delta: number) {
    this.cartService.updateQuantity(item.medicineId, item.quantity + delta);
  }

  removeItem(item: CartItem) {
    this.cartService.removeFromCart(item.medicineId);
  }

  checkout() {
    if (!this.authService.getToken()) {
      this.router.navigate(['/login']);
      return;
    }

    if (this.cartItems.length === 0) return;

    this.loading = true;
    this.error = '';

    const orderData = {
      notes: this.notes,
      items: this.cartItems.map(i => ({ medicineId: i.medicineId, quantity: i.quantity }))
    };

    this.orderService.placeOrder(orderData).subscribe({
      next: (res) => {
        this.loading = false;
        this.success = 'Order placed successfully!';
        this.cartService.clearCart();
        setTimeout(() => this.router.navigate(['/my-orders']), 1500);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Failed to place order.';
      }
    });
  }
}
