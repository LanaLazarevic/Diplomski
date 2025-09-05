import { Routes } from '@angular/router';
import {TransactionList} from './components/trancastion-list/transaction-list.component';
import {Login} from './components/login/login';
import {authGuard} from './guards/auth-guard';
import {CardList} from './components/card-list/card-list';
import {NewCard} from './components/new-card/new-card';
import {UserList} from './components/user-list/user-list';
import {UpdateUser} from './components/update-user/update-user';
import {CreateUser} from './components/create-user/create-user';
import {AccountList} from './components/account-list/account-list';
import {NewAccount} from './components/new-account/new-account';

export const routes: Routes =  [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'transactions', component: TransactionList, canActivate:[authGuard] },
  { path: 'cards', component: CardList, canActivate:[authGuard] },
  { path: 'cards/new', component: NewCard, canActivate:[authGuard] },
  { path: 'users', component: UserList, canActivate:[authGuard] },
  { path: 'users/create', component: CreateUser, canActivate:[authGuard] },
  { path: 'users/update/:id', component: UpdateUser, canActivate:[authGuard] },
  { path: 'accounts', component: AccountList, canActivate:[authGuard] },
  { path: 'accounts/create', component: NewAccount, canActivate:[authGuard] }
];
