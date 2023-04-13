import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { User } from './user';

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
    localStorage.setItem('token', 'testAuth');
  }

  logout() {
    this.loggedIn.next(false);
    this.router.navigate(['/auth/login']);
    localStorage.removeItem('token');
  }
}
