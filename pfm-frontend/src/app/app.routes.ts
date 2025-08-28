import { Routes } from '@angular/router';
import {TransactionList} from './components/trancastion-list/transaction-list.component';
import {Login} from './components/login/login';
import {authGuard} from './guards/auth-guard';

export const routes: Routes =  [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'transactions', component: TransactionList, canActivate:[authGuard] }
];
