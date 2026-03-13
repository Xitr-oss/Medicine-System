import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MedicineService } from '../../services/medicine.service';

@Component({
  selector: 'app-admin-medicines',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-medicines.component.html',
  styleUrls: ['./admin-medicines.component.scss']
})
export class AdminMedicinesComponent implements OnInit {
  medicineService = inject(MedicineService);

  medicines: any[] = [];
  categories: any[] = [];
  
  showModal = false;
  isEditMode = false;
  
  formData: any = {
    id: 0,
    name: '',
    categoryId: 0,
    price: 0,
    description: '',
    stock: 0,
    isActive: true
  };

  ngOnInit() {
    this.loadMedicines();
    this.loadCategories();
  }

  loadMedicines() {
    this.medicineService.getMedicines().subscribe(res => {
      this.medicines = res;
    });
  }

  loadCategories() {
    this.medicineService.getCategories().subscribe(res => {
      this.categories = res;
    });
  }

  openAddModal() {
    this.isEditMode = false;
    this.formData = {
      id: 0,
      name: '',
      categoryId: this.categories.length > 0 ? this.categories[0].id : 0,
      price: 0,
      description: '',
      stock: 0,
      isActive: true
    };
    this.showModal = true;
  }

  openEditModal(med: any) {
    this.isEditMode = true;
    this.formData = { ...med };
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  saveMedicine() {
    if (this.isEditMode) {
      this.medicineService.updateMedicine(this.formData.id, this.formData).subscribe(() => {
        this.loadMedicines();
        this.closeModal();
      });
    } else {
      this.medicineService.createMedicine(this.formData).subscribe(() => {
        this.loadMedicines();
        this.closeModal();
      });
    }
  }

  deleteMedicine(id: number) {
    if (confirm('Are you sure you want to delete this medicine?')) {
      this.medicineService.deleteMedicine(id).subscribe(() => {
        this.loadMedicines();
      });
    }
  }
}
