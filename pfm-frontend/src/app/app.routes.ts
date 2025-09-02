import { Routes } from '@angular/router';
import {TransactionList} from './components/trancastion-list/transaction-list.component';
import {Login} from './components/login/login';
import {authGuard} from './guards/auth-guard';
import {CardList} from './components/card-list/card-list';
import {NewCard} from './components/new-card/new-card';
import {UserList} from './components/user-list/user-list';

export const routes: Routes =  [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'transactions', component: TransactionList, canActivate:[authGuard] },
  { path: 'cards', component: CardList, canActivate:[authGuard] },
  { path: 'cards/new', component: NewCard, canActivate:[authGuard] },
  { path: 'users', component: UserList, canActivate:[authGuard] }
];
