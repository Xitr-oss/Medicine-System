import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-customer-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './customer-orders.component.html',
  styleUrls: ['./customer-orders.component.scss']
})
export class CustomerOrdersComponent implements OnInit {
  orderService = inject(OrderService);
  orders: any[] = [];
  loading = true;

  ngOnInit() {
    this.orderService.getMyOrders().subscribe({
      next: (res) => {
        this.orders = res;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
