import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:5001/api/auth'; // Ensure this matches typical ASP.NET HTTPS ports or use 5000 for HTTP

  private currentRoleSubject = new BehaviorSubject<string | null>(this.getRole());
  currentRole$ = this.currentRoleSubject.asObservable();

  private isLoggedInSubject = new BehaviorSubject<boolean>(!!this.getToken());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor() { }

  register(data: any) {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  login(data: any) {
    return this.http.post<any>(`${this.apiUrl}/login`, data).pipe(
      tap(res => {
        if (res && res.token) {
          localStorage.setItem('token', res.token);
          localStorage.setItem('role', res.role);
          localStorage.setItem('userId', res.userId.toString());
          localStorage.setItem('name', res.name);
          
          this.currentRoleSubject.next(res.role);
          this.isLoggedInSubject.next(true);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    localStorage.removeItem('userId');
    localStorage.removeItem('name');
    
    this.currentRoleSubject.next(null);
    this.isLoggedInSubject.next(false);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  getRole() {
    return localStorage.getItem('role');
  }

  getUserId() {
    return localStorage.getItem('userId');
  }
}
