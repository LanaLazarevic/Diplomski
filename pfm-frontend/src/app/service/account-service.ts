import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {AccountDto, AccountDtoRaw, PagedList, PagedListRaw} from '../model/model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private apiUrl = 'http://localhost:5156/accounts';

  constructor(private http: HttpClient) {}

  getAccounts(page: number = 1): Observable<PagedList<AccountDto>> {
    const params = new HttpParams().set('page', page.toString());
    return this.http
      .get<PagedListRaw<AccountDtoRaw>>(this.apiUrl, { params, headers: this.getHeaders() })
      .pipe(
        map(raw => ({
          items: raw.items.map(a => ({
            id: a.id,
            accountNumber: a['account-number'],
            availableAmount: a['available-amount'],
            reservedAmount: a['reserved-amount'],
            currency: a.currency,
            accountType: a['account-type'],
            isActive: a['is-active'],
            userFullName: a['user-full-name']
          })),
          totalCount: raw['total-count'],
          pageSize: raw['page-size'],
          page: raw.page,
          totalPages: raw['total-pages'],
          sortOrder: raw['sort-order'],
          sortBy: raw['sort-by']
        }))
      );
  }

  deactivateAccount(id: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/deactivate`, {}, {
      headers: this.getHeaders(),
      responseType: 'text'
    });
  }

  private getHeaders() {
    const jwt = sessionStorage.getItem('jwt');
    return new HttpHeaders().set('Authorization', `Bearer ${jwt}`);
  }
}
