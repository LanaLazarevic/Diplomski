import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {PagedList, PagedListRaw, UpdateUserDto, UserDto, UserDtoRaw} from '../model/model';

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

  updateUser(id: string, user: UpdateUserDto): Observable<any> {
    const body = {
      'first-name': user.firstName,
      'last-name': user.lastName,
      'email': user.email,
      'password': user.password,
      'address': user.address,
      'phone-number': user.phoneNumber
    };
    return this.http.put(`${this.apiUrl}/${id}`, body, {
      headers: this.getHeaders(),
      responseType: 'text'
    });
  }

  private getHeaders() {
    const jwt = sessionStorage.getItem('jwt');
    return new HttpHeaders().set('Authorization', `Bearer ${jwt}`);
  }
}
