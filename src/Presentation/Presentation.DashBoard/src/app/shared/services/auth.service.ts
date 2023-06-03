import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, from, of, tap } from 'rxjs';

@Injectable()
export class AuthService {
  private isLoggedInSubject: BehaviorSubject<boolean>;

  get isLoggedIn(): Observable<boolean> {
    return this.isLoggedInSubject.asObservable();
  }

  constructor(private router: Router) {
    const initialLoginStatus = localStorage.getItem('token') !== null;
    this.isLoggedInSubject = new BehaviorSubject<boolean>(initialLoginStatus);
    console.log('constructor isLoggedInSubject', this.isLoggedInSubject);
  }

  login(token: string | null): Observable<boolean> {
    console.log('login method called with token:', token);
    // Logic for validating the token and logging the user in
    const isValidToken = this.validateToken(token);

    if (isValidToken) {
      // Token is valid, mark the user as logged in
      this.isLoggedInSubject.next(true);
      this.router.navigate(['/home']);

      return of(true);
    } else {
      // Token is invalid, mark the user as not logged in
      this.isLoggedInSubject.next(false);
      return of(false);
    }
  }

  logout(): void {
    this.isLoggedInSubject.next(false);
    this.router.navigate(['/auth/login']);
    localStorage.removeItem('token');
    localStorage.removeItem('expiresAt');
  }

  private validateToken(token: string | null): boolean {
    console.log('validateToken method called with token:', token);

    if (!token) {
      return false; // Token is not provided or null
    }

    const expirationDate = new Date(localStorage.getItem('expiresAt')!);
    const currentDate = new Date();

    if (currentDate > expirationDate) {
      return false; // Token has expired
    }

    return true; // Token is valid
  }
}
