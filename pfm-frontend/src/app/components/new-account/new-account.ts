import { Component } from '@angular/core';
import {AccountService} from '../../service/account-service';
import {Router} from '@angular/router';
import {FormsModule, NgForm} from '@angular/forms';
import {Sidebar} from '../sidebar/sidebar';
import {SidebarService} from '../../service/sidebar-service';

@Component({
  selector: 'app-new-account',
  imports: [
    Sidebar,
    FormsModule,
  ],
  templateUrl: './new-account.html',
  styleUrl: './new-account.css'
})
export class NewAccount {
  account = {
    accountNumber: '',
    availableAmount: 0,
    reservedAmount: 0,
    currency: '',
    userJmbg: '',
    accountType: 'checking'
  };

  constructor(private service: AccountService, private router: Router, public sidebarService: SidebarService) {}

  onSubmit(form: NgForm) {
    if (form.invalid) {
      form.form.markAllAsTouched();
      return;
    }
    const payload = {
      accountNumber: Number(this.account.accountNumber),
      availableAmount: this.account.availableAmount,
      reservedAmount: this.account.reservedAmount,
      currency: this.account.currency,
      userJmbg: this.account.userJmbg,
      accountType: this.account.accountType
    };
    this.service.createAccount(payload).subscribe({
      next: () => this.router.navigate(['/accounts']),
      error: err => console.error('Error creating account:', err)
    });
  }

  backToAccountList() {
    this.router.navigate(['/accounts']);
  }
}
