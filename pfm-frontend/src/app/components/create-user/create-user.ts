import { Component } from '@angular/core';
import {CreateUserDto} from '../../model/model';
import {UserService} from '../../service/user-service';
import {Router} from '@angular/router';
import {FormsModule, NgForm} from '@angular/forms';
import {Sidebar} from '../sidebar/sidebar';
import {SidebarService} from '../../service/sidebar-service';
import {AsyncPipe, NgClass} from '@angular/common';

@Component({
  selector: 'app-create-user',
  imports: [
    Sidebar,
    FormsModule,
    AsyncPipe,
    NgClass
  ],
  templateUrl: './create-user.html',
  styleUrl: './create-user.css'
})
export class CreateUser {
  user: CreateUserDto = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    address: '',
    phoneNumber: '',
    birthday: '',
    role: 'user',
    jmbg: ''
  };
  maxDate: string;
  error: string | null = null;

  constructor(private service: UserService, private router: Router, public sidebarService: SidebarService) {
    const today = new Date();
    today.setFullYear(today.getFullYear() - 18);
    this.maxDate = today.toISOString().split('T')[0];
  }

  onSubmit(form: NgForm) {
    if (form.invalid) {
      form.form.markAllAsTouched();
      return;
    }
    const birthdayDate = new Date(this.user.birthday);
    const cutoff = new Date();
    cutoff.setFullYear(cutoff.getFullYear() - 18);
    if (birthdayDate > cutoff) {
      this.error = 'User must be at least 18 years old';
      return;
    }
    this.service.createUser(this.user).subscribe({
      next: () => this.router.navigate(['/users']),
      error: err => {
        console.error('Error creating user', err);
        this.error = 'Failed to create user';
      }
    });
  }

  backToUsers() {
    this.router.navigate(['/users']);
  }
}
