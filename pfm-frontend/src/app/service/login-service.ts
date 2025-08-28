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
    this.setUserRole(decoded.role);
    if (decoded.email) {
      sessionStorage.setItem('email', decoded.email);
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
}
