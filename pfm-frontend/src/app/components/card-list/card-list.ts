import {Component, OnInit} from '@angular/core';
import {Sidebar} from '../sidebar/sidebar';
import {NgClass} from '@angular/common';
import {CardDto, PagedList} from '../../model/model';
import {CardService} from '../../service/card-service';
import {LoginService} from '../../service/login-service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-card-list',
  imports: [Sidebar, NgClass],
  templateUrl: './card-list.html',
  styleUrl: './card-list.css'
})
export class CardList implements OnInit {
  pagedCards: PagedList<CardDto> | null = null;
  loading = false;
  error = false;

  constructor(private service: CardService, private loginService: LoginService, private router: Router) {
  }

  ngOnInit() {
    this.loadCards();
  }

  loadCards(page: number = 1) {
    this.loading = true;
    this.error = false;
    this.service.getCards(page).subscribe({
      next: res => {
        this.pagedCards = res;
        this.loading = false;
      },
      error: _ => {
        this.error = true;
        this.loading = false;
      }
    });
  }

  isAdmin(): boolean {
    return this.loginService.isAdmin();
  }

  deactivate(cardId: string) {
    this.service.deactivateCard(cardId).subscribe({
      next: () => this.loadCards(this.pagedCards?.page || 1),
      error: err => console.error('Error deactivating card:', err)
    });
  }

  formatCardNumber(num: string | number): string {
    const digits = String(num).replace(/\D/g, '');
    if (digits.length <= 8) {
      return digits;
    }
    const first4 = digits.slice(0, 4);
    const last4 = digits.slice(-4);
    const middle = '*'.repeat(digits.length - 8);
    const grouped = (first4 + middle + last4).match(/.{1,4}/g);
    return grouped ? grouped.join(' ') : first4 + middle + last4;
  }

  formatAmount(amount: number): string {
    return new Intl.NumberFormat('sr-RS', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    }).format(amount);
  }

  openNewCardForm() {
    this.router.navigate(['/cards/new']);
  }
}
