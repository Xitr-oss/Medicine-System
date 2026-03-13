import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class MedicineService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5000/api/medicines';

  constructor() { }

  getMedicines(categoryId?: number, search?: string) {
    let params = new HttpParams();
    if (categoryId) {
      params = params.set('categoryId', categoryId.toString());
    }
    if (search) {
      params = params.set('search', search);
    }
    return this.http.get<any[]>(this.apiUrl, { params });
  }

  getMedicine(id: number) {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  createMedicine(data: any) {
    return this.http.post<any>(this.apiUrl, data);
  }

  updateMedicine(id: number, data: any) {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  deleteMedicine(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getCategories() {
    return this.http.get<any[]>(`${this.apiUrl}/categories`);
  }
}
