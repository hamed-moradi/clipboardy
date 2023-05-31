import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { IUser } from 'src/app/auth/IUser';
import { AuthService } from 'src/app/auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class SignInService {
  constructor(
    private httpClient: HttpClient,
    private authService: AuthService
  ) {}

  signIn(
    AccountKey: string,
    Password: string,
    RememberMe: boolean
  ): Observable<{ success: boolean }> {
    //URL
    const baseURL: string = 'http://localhost:2020';

    //body
    const body: IUser = {
      AccountKey,
      Password,
      RememberMe,
    };

    this.authService.login();

    return this.httpClient
      .post<{ success: boolean; expiresAt: string; authorization: string }>(
        baseURL + '/api/account/signin',
        body
      )
      .pipe(
        tap((response) => {
          //store the expiresAt and token values in localStorage
          localStorage.setItem('expiresAt', response.expiresAt);
          localStorage.setItem('authorization', response.authorization);
        })
      );
  }
}
