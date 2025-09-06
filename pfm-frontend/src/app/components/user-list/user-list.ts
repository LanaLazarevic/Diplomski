import {Component, OnInit} from '@angular/core';
import {PagedList, UserDto} from '../../model/model';
import {UserService} from '../../service/user-service';
import {LoginService} from '../../service/login-service';
import {Sidebar} from '../sidebar/sidebar';
import {AsyncPipe, NgClass} from '@angular/common';
import {Router} from '@angular/router';
import {SidebarService} from '../../service/sidebar-service';

@Component({
  selector: 'app-user-list',
  imports: [
    Sidebar,
    NgClass,
    AsyncPipe
  ],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList implements OnInit {
  pagedUsers: PagedList<UserDto> | null = null;
  loading = false;
  error = false;

  constructor(private service: UserService, private loginService: LoginService, private router: Router, public sidebarService: SidebarService) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers(page: number = 1) {
    this.loading = true;
    this.error = false;
    this.service.getUsers(page).subscribe({
      next: res => {
        this.pagedUsers = res;
        this.loading = false;
      },
      error: _ => {
        this.error = true;
        this.loading = false;
      }
    });
  }

  updateUser(user: UserDto) {
    this.router.navigate(['/users/update', user.id], { state: { user } });
  }

  createUser() {
    this.router.navigate(['/users/create']);
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString();
  }
}
