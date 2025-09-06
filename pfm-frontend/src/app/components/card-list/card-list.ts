import {Component, OnInit} from '@angular/core';
import {Sidebar} from '../sidebar/sidebar';
import { NgClass} from '@angular/common';
import {CardDto, PagedList} from '../../model/model';
import {CardService} from '../../service/card-service';
import {LoginService} from '../../service/login-service';
import {Router} from '@angular/router';
import {FormsModule} from '@angular/forms';
import {SidebarService} from '../../service/sidebar-service';

@Component({
  selector: 'app-card-list',
  imports: [Sidebar, NgClass, FormsModule],
  templateUrl: './card-list.html',
  styleUrl: './card-list.css'
})
export class CardList implements OnInit {
  pagedCards: PagedList<CardDto> | null = null;
  groupedCards: { userId: string; ownerName: string; cards: CardDto[] }[] = [];
  loading = false;
  error = false;
  currentPage = 1;
  filterJmbg = '';

  constructor(private service: CardService, private loginService: LoginService, private router: Router, public sidebarService: SidebarService) {
  }

  ngOnInit() {
    this.loadCards();
  }

  loadCards(page: number = 1) {
    this.loading = true;
    this.error = false;
    const params = this.filterJmbg ? { 'user-jmbg': this.filterJmbg } : undefined;
    this.service.getCards(page, params).subscribe({
      next: res => {
        this.pagedCards = res;
        this.groupCards();
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

  previousPage() {
    if (this.currentPage > 1) {
      this.loadCards(this.currentPage - 1);
      this.currentPage -= 1;
    }
  }

  nextPage() {
    if (this.pagedCards && this.currentPage < this.pagedCards.totalPages) {
      this.loadCards(this.currentPage + 1);
      this.currentPage += 1;
    }
  }

  goToPage(page: number) {
    if (page !== this.currentPage && this.pagedCards && page >= 1 && page <= this.pagedCards.totalPages) {
      this.loadCards(page);
      this.currentPage = page;
    }
  }

  getVisiblePages(): number[] {
    if (!this.pagedCards) return [];

    const totalPages = this.pagedCards.totalPages;
    const current = this.currentPage;
    const pages: number[] = [];

    if (totalPages <= 7) {
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      pages.push(1);

      if (current <= 4) {
        for (let i = 2; i <= 5; i++) {
          pages.push(i);
        }
        pages.push(-1);
        pages.push(totalPages);
      } else if (current >= totalPages - 3) {
        pages.push(-1);
        for (let i = totalPages - 4; i <= totalPages; i++) {
          pages.push(i);
        }
      } else {
        pages.push(-1);
        for (let i = current - 1; i <= current + 1; i++) {
          pages.push(i);
        }
        pages.push(-1);
        pages.push(totalPages);
      }
    }

    return pages;
  }

  formatAccountNumber(num: string | number): string {
    const digits = String(num).replace(/\D/g, '');
    return digits.replace(/(\d{3})(?=\d)/g, '$1 ');
  }

  private groupCards() {
    if (!this.pagedCards) {
      this.groupedCards = [];
      return;
    }

    const map = new Map<string, { userId: string; ownerName: string; cards: CardDto[] }>();
    for (const card of this.pagedCards.items) {
      const group = map.get(card.userId);
      if (group) {
        group.cards.push(card);
      } else {
        map.set(card.userId, {
          userId: card.userId,
          ownerName: card.ownerName,
          cards: [card]
        });
      }
    }
    this.groupedCards = Array.from(map.values());
  }

  applyFilter() {
    this.currentPage = 1;
    this.loadCards();
  }
}
