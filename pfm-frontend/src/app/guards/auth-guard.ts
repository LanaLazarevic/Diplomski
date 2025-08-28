import {CanActivateFn, Router} from '@angular/router';
import {LoginService} from '../service/login-service';
import {inject} from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const loginService = inject(LoginService);
  const router = inject(Router);
  const jwt = loginService.getToken();
  const isAdmin = loginService.isAdmin();

  if (!jwt) {
    alert('You have to be logged in.');
    return router.createUrlTree(['/login']);
  }

  return true;
};
