import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, from, of, tap } from "rxjs";

@Injectable()
export class AuthService {
  private isLoggedInSubject: BehaviorSubject<boolean>;

  get isLoggedIn(): Observable<boolean> {
    return this.isLoggedInSubject.asObservable();
  }

  constructor(private router: Router, private httpClient: HttpClient) {
    const initialLoginStatus =
      (localStorage.getItem("token") || sessionStorage.getItem("token")) !==
      null;
    this.isLoggedInSubject = new BehaviorSubject<boolean>(initialLoginStatus);
    // console.log("constructor isLoggedInSubject", this.isLoggedInSubject);
  }

  login(token: string | null): Observable<boolean> {
    // Logic for validating the token and logging the user in
    const isValidToken = this.validateToken(token);

    //console.log("login method called with token:", isValidToken);
    if (isValidToken) {
      // Token is valid, mark the user as logged in
      this.isLoggedInSubject.next(true);
      this.router.navigate(["/home"]);
      return of(true);
    } else {
      // Token is invalid, mark the user as not logged in
      this.isLoggedInSubject.next(false);
      return of(false);
    }
  }

  logout(): void {
    this.isLoggedInSubject.next(false);
    this.router.navigate(["/auth/login"]);
    localStorage.removeItem("token");
    localStorage.removeItem("expiresAt");

    sessionStorage.removeItem("token");
    sessionStorage.removeItem("expiresAt");
  }

  private validateToken(token: string | null): boolean {
    //console.log("validateToken method called with token:", token);

    if (!token) {
      return false; // Token is not provided or null
    }

    const EpochTimeStampOfcurrentDate = new Date().getTime() / 1000;
    const JWT = token;
    const jwtPayload = JSON.parse(window.atob(JWT.split(".")[1]));

    if (EpochTimeStampOfcurrentDate > jwtPayload.exp) {
      return false; // Token has expired
    }

    return true; // Token is valid
  }
}
