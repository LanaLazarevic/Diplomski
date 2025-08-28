import { Component } from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {LoginService} from '../../service/login-service';

@Component({
  selector: 'app-sidebar',
  imports: [
    RouterLink
  ],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class Sidebar {
  constructor(public loginService: LoginService, private router: Router) {}

  isAdmin(){
    return this.loginService.isAdmin();
  }

  getUserName(){
    return this.loginService.getUserName();
  }

  getUserInitials(){
    return this.loginService.getUserInitials();
  }

  logout(): void {
    this.loginService.clearAuthData();
    this.router.navigate(['/login']);
  }

}
