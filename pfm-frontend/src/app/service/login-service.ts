import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {BusinessError, LoginResponse} from '../model/model';
import {BehaviorSubject, catchError, Observable, of, tap, throwError} from 'rxjs';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private apiUrl = 'http://localhost:5156/login';
  private roleSubject = new BehaviorSubject<string | null>(this.getUserRole());
  role$ = this.roleSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.apiUrl, { email, password }).pipe(
      tap(res => {
        this.setToken(res.jwt);
      })
    );
  }

  setToken(jwt: string): void {
    sessionStorage.setItem('jwt', jwt);
    const decoded: any = jwtDecode(jwt);
    const role =
      decoded.role ||
      decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    const email =
      decoded.email ||
      decoded[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
        ];
    const userId =
      decoded.sub ||
      decoded[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
        ];
    const fullName =
      decoded.name ||
      decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];

    if (role) {
      this.setUserRole(role);
    }
    if (fullName) {
      sessionStorage.setItem('fullName', fullName);
    }

  }

  setUserRole(role: string): void {
    sessionStorage.setItem('role', role);
    sessionStorage.setItem('isAdmin', (role?.toLowerCase() === 'admin').toString());
    this.roleSubject.next(role);
  }

  getUserRole(): string | null {
    return sessionStorage.getItem('role');
  }

  getToken(): string | null {
    return sessionStorage.getItem('jwt');
  }

  isAdmin(): boolean {
    return sessionStorage.getItem('isAdmin') === 'true';
  }

  getUserName(): string | null {
    return sessionStorage.getItem('fullName');
  }

  getUserInitials(): string {
    const name = this.getUserName();
    if (!name) {
      return '';
    }
    return name
      .split(' ')
      .map(part => part.charAt(0))
      .join('')
      .toUpperCase();
  }

  clearAuthData(): void {
    sessionStorage.removeItem('jwt');
    sessionStorage.removeItem('role');
    sessionStorage.removeItem('isAdmin');
    sessionStorage.removeItem('email');
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('fullName');
    this.roleSubject.next(null);
  }
}
