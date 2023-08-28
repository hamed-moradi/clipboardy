import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { map, take, tap } from 'rxjs/operators';

import { AuthService } from '../shared/services/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.authService.isLoggedIn.pipe(
      take(1),
      tap((r) => console.log(r)),
      map((isLoggedIn) => {
        if (isLoggedIn) {
          console.log('User is logged in');
          return true;
        } else {
          console.log('User is not logged in');
          this.router.navigate(['auth/login']);
          return false;
        }
      })
    );
  }
}
