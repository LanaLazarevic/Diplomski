import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {CardDto, CardDtoRaw, CreateCardDto, PagedList, PagedListRaw} from '../model/model';

@Injectable({
  providedIn: 'root'
})
export class CardService {
  private apiUrl = 'http://localhost:5156/cards';

  constructor(private http: HttpClient) {}

  getCards(page: number = 1, params?: any): Observable<PagedList<CardDto>> {
    let queryParams = new HttpParams().set('page', page.toString());
    if (params) {
      for (const key in params) {
        if (params[key] !== undefined && params[key] !== null) {
          queryParams = queryParams.set(key, params[key]);
        }
      }
    }
    return this.http
      .get<PagedListRaw<CardDtoRaw>>(this.apiUrl, {
        params: queryParams,
        headers: this.getHeaders()
      })
      .pipe(
        map(raw => ({
          items: raw.items.map(r => ({
            id: r.id,
            ownerName: r['owner-name'],
            cardNumber: r['card-number'],
            expirationDate: r['expiration-date'],
            availableAmount: r['available-amount'],
            reservedAmount: r['reserved-amount'],
            accountNumber: r['account-number'],
            cardType: r['card-type'],
            userId: r['user-id'],
            isActive: r['is-active']
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

  deactivateCard(id: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/deactivate`, {}, {
      headers: this.getHeaders(),
      responseType: 'text'
    });
  }

  private getHeaders() {
    const jwt = sessionStorage.getItem('jwt');
    return new HttpHeaders().set('Authorization', `Bearer ${jwt}`);
  }

  createCard(card: CreateCardDto): Observable<any> {
    const body = {
      'owner-name': card.ownerName,
      'card-number': card.cardNumber,
      'expiration-date': card.expirationDate,
      'available-amount': card.availableAmount,
      'reserved-amount': card.reservedAmount,
      'user-jmbg': card.userJmbg,
      'account-number': card.accountNumber,
      'card-type': card.cardType
    };
    return this.http.post(this.apiUrl, body, {
      headers: this.getHeaders(),
      responseType: 'text'
    });
  }

}
