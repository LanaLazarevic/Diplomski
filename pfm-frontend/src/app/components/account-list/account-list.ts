import {Component, OnInit} from '@angular/core';
import {AccountDto, PagedList} from '../../model/model';
import {AccountService} from '../../service/account-service';
import {LoginService} from '../../service/login-service';
import {Sidebar} from '../sidebar/sidebar';
import {NgClass} from '@angular/common';

@Component({
  selector: 'app-account-list',
  imports: [
    Sidebar,
    NgClass
  ],
  templateUrl: './account-list.html',
  styleUrl: './account-list.css'
})
export class AccountList implements OnInit {
  pagedAccounts: PagedList<AccountDto> | null = null;
  loading = false;
  error = false;
  currentPage = 1;
  constructor(private service: AccountService, private loginService: LoginService) {}

  ngOnInit() {
    this.loadAccounts();
  }

  loadAccounts(page: number = 1) {
    this.loading = true;
    this.error = false;
    this.service.getAccounts(page).subscribe({
      next: res => {
        this.pagedAccounts = res;
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

  deactivate(id: string) {
    this.service.deactivateAccount(id).subscribe({
      next: () => this.loadAccounts(this.pagedAccounts?.page || 1),
      error: err => console.error('Error deactivating account:', err)
    });
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.loadAccounts(this.currentPage - 1);
      this.currentPage -= 1;
    }
  }

  nextPage() {
    if (this.pagedAccounts && this.currentPage < this.pagedAccounts.totalPages) {
      this.loadAccounts(this.currentPage + 1);
      this.currentPage += 1;
    }
  }

  goToPage(page: number) {
    if (page !== this.currentPage && this.pagedAccounts && page >= 1 && page <= this.pagedAccounts.totalPages) {
      this.loadAccounts(page);
      this.currentPage = page;
    }
  }

  getVisiblePages(): number[] {
    if (!this.pagedAccounts) return [];

    const totalPages = this.pagedAccounts.totalPages;
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

  formatAmount(amount: number): string {
    return new Intl.NumberFormat('sr-RS', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    }).format(amount);
  }
}
