import { Component } from '@angular/core';
import {CardService} from '../../service/card-service';
import {Router} from '@angular/router';
import {FormsModule, NgForm} from '@angular/forms';
import {Sidebar} from '../sidebar/sidebar';

@Component({
  selector: 'app-new-card',
  imports: [
    Sidebar,
    FormsModule
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
    cardType: 'debit'
  };

  constructor(private service: CardService, private router: Router) {}

  onSubmit(form: NgForm) {
    if (form.invalid) {
      form.form.markAllAsTouched();
      return;
    }
    this.service.createCard(this.card).subscribe({
      next: () => this.router.navigate(['/cards']),
      error: err => console.error('Error creating card:', err)
    });
  }

  backToCardList(){
    this.router.navigate(['/cards']);
  }
}
