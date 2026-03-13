import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  orderService = inject(OrderService);
  stats: any = null;
  orders: any[] = [];
  loading = true;

  ngOnInit() {
    this.orderService.getDashboardStats().subscribe(res => {
      this.stats = res;
      this.loadRecentOrders();
    });
  }

  loadRecentOrders() {
    this.orderService.getAllOrders().subscribe(res => {
      this.orders = res;
      this.loading = false;
    });
  }

  updateStatus(order: any, ev: Event) {
    const status = (ev.target as HTMLSelectElement).value;
    this.orderService.updateOrderStatus(order.id, status).subscribe(() => {
      order.status = status;
    });
  }
}
