import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MedicineService } from '../../services/medicine.service';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './catalog.component.html',
  styleUrls: ['./catalog.component.scss']
})
export class CatalogComponent implements OnInit {
  medicineService = inject(MedicineService);
  cartService = inject(CartService);

  medicines: any[] = [];
  categories: any[] = [];
  selectedCategoryId: number | null = null;
  searchQuery: string = '';
  loading = true;

  ngOnInit(): void {
    this.loadCategories();
    this.loadMedicines();
  }

  loadCategories() {
    this.medicineService.getCategories().subscribe(res => {
      this.categories = res;
    });
  }

  loadMedicines() {
    this.loading = true;
    this.medicineService.getMedicines(this.selectedCategoryId ?? undefined, this.searchQuery).subscribe(res => {
      this.medicines = res.filter(m => m.isActive && m.stock > 0);
      this.loading = false;
    });
  }

  filterByCategory(catId: number | null) {
    this.selectedCategoryId = catId;
    this.loadMedicines();
  }

  onSearch() {
    this.loadMedicines();
  }

  addToCart(med: any) {
    this.cartService.addToCart(med, 1);
  }
}
