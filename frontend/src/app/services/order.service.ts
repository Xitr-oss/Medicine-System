import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5000/api/orders';

  constructor() { }

  placeOrder(data: any) {
    return this.http.post<any>(this.apiUrl, data);
  }

  getMyOrders() {
    return this.http.get<any[]>(`${this.apiUrl}/my-orders`);
  }

  getAllOrders() {
    return this.http.get<any[]>(this.apiUrl);
  }

  updateOrderStatus(id: number, status: string) {
    return this.http.put(`${this.apiUrl}/${id}/status`, { status });
  }

  getDashboardStats() {
    return this.http.get<any>(`${this.apiUrl.replace('/orders', '')}/dashboard/stats`);
  }
}
