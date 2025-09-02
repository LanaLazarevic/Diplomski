import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {PagedList, PagedListRaw, UserDto, UserDtoRaw} from '../model/model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5156/users';

  constructor(private http: HttpClient) {}

  getUsers(page: number = 1): Observable<PagedList<UserDto>> {
    const params = new HttpParams().set('page', page.toString());
    return this.http
      .get<PagedListRaw<UserDtoRaw>>(this.apiUrl, {
        params,
        headers: this.getHeaders()
      })
      .pipe(
        map(raw => ({
          items: raw.items.map(r => ({
            id: r.id,
            firstName: r['first-name'],
            lastName: r['last-name'],
            email: r.email,
            address: r.address,
            phoneNumber: r['phone-number'],
            birthday: r.birthday,
            jmbg: r.jmbg,
            role: r.role
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

  private getHeaders() {
    const jwt = sessionStorage.getItem('jwt');
    return new HttpHeaders().set('Authorization', `Bearer ${jwt}`);
  }
}
