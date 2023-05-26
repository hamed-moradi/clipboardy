import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class AuthService {
  private loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false
  );

  get isLoggedIn() {
    return this.loggedIn;
  }

  constructor(private router: Router) {}

  login() {
    this.loggedIn.next(true);
    this.router.navigate(['/']);
  }

  logout() {
    this.loggedIn.next(false);
    this.router.navigate(['/auth/login']);
    localStorage.removeItem('token');
    localStorage.removeItem('expiresAt');
  }
}
