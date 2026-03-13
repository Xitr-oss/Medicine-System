import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface CartItem {
  medicineId: number;
  name: string;
  price: number;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItems: CartItem[] = [];
  private cartSubject = new BehaviorSubject<CartItem[]>(this.cartItems);
  
  cart$ = this.cartSubject.asObservable();

  constructor() { }

  addToCart(medicine: any, quantity: number = 1) {
    const existingItem = this.cartItems.find(i => i.medicineId === medicine.id);
    if (existingItem) {
      existingItem.quantity += quantity;
    } else {
      this.cartItems.push({
        medicineId: medicine.id,
        name: medicine.name,
        price: medicine.price,
        quantity: quantity
      });
    }
    this.updateCart();
  }

  removeFromCart(medicineId: number) {
    this.cartItems = this.cartItems.filter(i => i.medicineId !== medicineId);
    this.updateCart();
  }

  updateQuantity(medicineId: number, quantity: number) {
    const item = this.cartItems.find(i => i.medicineId === medicineId);
    if (item) {
      item.quantity = quantity;
      if (item.quantity <= 0) {
        this.removeFromCart(medicineId);
      } else {
        this.updateCart();
      }
    }
  }

  clearCart() {
    this.cartItems = [];
    this.updateCart();
  }

  getTotal() {
    return this.cartItems.reduce((acc, item) => acc + (item.price * item.quantity), 0);
  }

  getItems() {
    return this.cartItems;
  }

  private updateCart() {
    this.cartSubject.next([...this.cartItems]);
  }
}
