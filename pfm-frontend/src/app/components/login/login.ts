import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {LoginService} from '../../service/login-service';
import {BusinessError} from '../../model/model';
import {Router} from '@angular/router';
import {SidebarService} from '../../service/sidebar-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl: './login.css',
  imports: [FormsModule]
})
export class Login {
  email = '';
  password = '';
  error: BusinessError | null = null;

  constructor(private loginService: LoginService, private router: Router, public sidebarService: SidebarService) {}

  onSubmit() {
    this.error = null;
    this.loginService.login(this.email, this.password).subscribe({
      next: () => {
        this.router.navigate(['/transactions']);
      },
      error: err => {
        const errors = Array.isArray(err.error) ? err.error : [err.error];
        const errBody: BusinessError = errors[0] || {};
        this.error = {
          problem: errBody.problem || 'auth',
          message: errBody.message || 'Login failed',
          details: errBody.details || 'An error occurred'
        };
      }
    });
  }
}
