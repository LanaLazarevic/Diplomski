import {Component, OnInit} from '@angular/core';
import {UpdateUserDto, UserDto} from '../../model/model';
import {ActivatedRoute, Router} from '@angular/router';
import {UserService} from '../../service/user-service';
import {FormsModule, NgForm} from '@angular/forms';
import {Sidebar} from '../sidebar/sidebar';

@Component({
  selector: 'app-update-user',
  imports: [
    Sidebar,
    FormsModule
  ],
  templateUrl: './update-user.html',
  styleUrl: './update-user.css'
})
export class UpdateUser implements OnInit {
  userId = '';
  user: UpdateUserDto = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    address: '',
    phoneNumber: ''
  };
  error: string | null = null;

  constructor(private route: ActivatedRoute, private service: UserService, private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    const stateUser = navigation?.extras.state?.['user'] as UserDto | undefined;
    if (stateUser) {
      this.user.firstName = stateUser.firstName;
      this.user.lastName = stateUser.lastName;
      this.user.email = stateUser.email;
      this.user.address = stateUser.address || '';
      this.user.phoneNumber = stateUser.phoneNumber || '';
    }
  }

  ngOnInit() {
    this.userId = this.route.snapshot.paramMap.get('id') || '';
  }

  onSubmit(form: NgForm) {
    if (form.invalid) {
      form.form.markAllAsTouched();
      return;
    }
    this.service.updateUser(this.userId, this.user).subscribe({
      next: () => this.router.navigate(['/users']),
      error: err => {
        console.error('Error updating user', err);
        this.error = 'Failed to update user';
      }
    });
  }

  backToUsers() {
    this.router.navigate(['/users']);
  }
}
