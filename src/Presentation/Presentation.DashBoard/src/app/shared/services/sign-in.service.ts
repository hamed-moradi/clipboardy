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

    //headers
    const headers = new HttpHeaders({
      deviceKey: this.generateDeviceKey(),
      deviceName: navigator.userAgent,
      deviceType: this.getDeviceType(),
    });

    //body
    const body: IUser = {
      AccountKey,
      Password,
      RememberMe,
    };

    this.authService.login();
    return this.httpClient
      .post<{ success: boolean; expiresAt: string; token: string }>(
        baseURL + '/api/account/signin',
        body,
        { headers: headers }
      )
      .pipe(
        tap((response) => {
          //store the expiresAt and token values in localStorage
          localStorage.setItem('expiresAt', response.expiresAt);
          localStorage.setItem('token', response.token);
        })
      );
  }

  private generateDeviceKey(): string {
    const userAgent = navigator.userAgent;
    const hash = this.hashString(userAgent);
    return hash;
  }

  private hashString(str: string): string {
    let hash = 0;
    if (str.length == 0) {
      return hash.toString();
    }
    for (let i = 0; i < str.length; i++) {
      let char = str.charCodeAt(i);
      hash = (hash << 5) - hash + char;
      hash = hash & hash; // Convert to 32bit integer
    }
    return hash.toString();
  }

  private getDeviceType(): string {
    const userAgent = navigator.userAgent;
    if (/iPad|iPhone|iPod/.test(userAgent)) {
      return 'iOS';
    } else if (/Android/.test(userAgent)) {
      return 'Android';
    } else {
      return 'PC';
    }
  }
}
