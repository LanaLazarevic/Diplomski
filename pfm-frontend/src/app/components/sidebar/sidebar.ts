import {Component, OnInit} from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {LoginService} from '../../service/login-service';
import {SidebarService} from '../../service/sidebar-service';
import {AsyncPipe} from '@angular/common';

@Component({
  selector: 'app-sidebar',
  imports: [
    RouterLink,
    AsyncPipe
  ],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class Sidebar implements OnInit {
  constructor(
    public loginService: LoginService,
    private router: Router,
    public sidebarService: SidebarService
  ) {}

  ngOnInit() {
    if (window.innerWidth >= 768) {
      this.sidebarService.open();
    }
  }

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

  toggleSidebar(): void {
    this.sidebarService.toggle();
  }

}
