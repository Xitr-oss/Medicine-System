import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  authService = inject(AuthService);
  cartService = inject(CartService);
  router = inject(Router);

  cartItemCount: number = 0;

  constructor() {
    this.cartService.cart$.subscribe(items => {
      this.cartItemCount = items.reduce((acc, item) => acc + item.quantity, 0);
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/catalog']);
  }
}
