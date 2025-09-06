import { Component } from '@angular/core';
import {CardService} from '../../service/card-service';
import {Router} from '@angular/router';
import {FormsModule, NgForm} from '@angular/forms';
import {Sidebar} from '../sidebar/sidebar';
import {SidebarService} from '../../service/sidebar-service';
import {AsyncPipe, NgClass} from '@angular/common';

@Component({
  selector: 'app-new-card',
  imports: [
    Sidebar,
    FormsModule,
    AsyncPipe,
    NgClass
  ],
  templateUrl: './new-card.html',
  styleUrl: './new-card.css'
})
export class NewCard {
  card = {
    ownerName: '',
    cardNumber: '',
    expirationDate: '',
    availableAmount: 0,
    reservedAmount: 0,
    userJmbg: '',
    accountNumber: 0,
    cardType: 'debit'
  };

  constructor(private service: CardService, private router: Router, public sidebarService: SidebarService) {}

  onSubmit(form: NgForm) {
    if (form.invalid) {
      form.form.markAllAsTouched();
      return;
    }
    const payload = {
      ...this.card,
      accountNumber: Number(this.card.accountNumber)
    };
    this.service.createCard(payload).subscribe({
      next: () => this.router.navigate(['/cards']),
      error: err => console.error('Error creating card:', err)
    });
  }

  backToCardList(){
    this.router.navigate(['/cards']);
  }
}
