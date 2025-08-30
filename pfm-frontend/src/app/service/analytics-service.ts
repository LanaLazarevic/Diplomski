import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {SpendingAnalyticsItem, SpendingAnalyticsResponse} from '../model/model';

@Injectable({
  providedIn: 'root'
})
export class AnalyticsService {
  private apiUrl = 'http://localhost:5156/spending-analytics';

  constructor(private http: HttpClient) {}

  getSpendingAnalytics(catcode?: string, startDate?: string | null, endDate?: string | null): Observable<SpendingAnalyticsItem[]> {
    let params = new HttpParams();
    if (catcode) {
      params = params.set('catcode', catcode);
    }
    if (startDate) {
      params = params.set('start-date', new Date(startDate).toISOString());
    }
    if (endDate) {
      params = params.set('end-date', new Date(endDate).toISOString());
    }
    return this.http
      .get<SpendingAnalyticsResponse>(this.apiUrl, { headers: this.getHeaders(), params })
      .pipe(
        map(res => {
          const total = res.groups.reduce((sum, g) => sum + g.amount, 0);
          return res.groups.map(g => ({
            catcode: g.catcode,
            name: g.catcode || 'Uncategorized',
            percentage: total > 0 ? (g.amount / total) * 100 : 0
          }));
        })
      );
  }

  private getHeaders() {
    const jwt = sessionStorage.getItem('jwt');
    return new HttpHeaders().set('Authorization', `Bearer ${jwt}`);
  }
}
