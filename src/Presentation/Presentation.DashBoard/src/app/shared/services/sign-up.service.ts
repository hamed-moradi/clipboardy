import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IUser } from 'src/app/auth/IUser';
import { Observable, tap } from 'rxjs';
import { Tab } from 'bootstrap';

@Injectable({
  providedIn: 'root',
})
export class SignUpService {
  constructor(private httpClient: HttpClient) {}

  signUp(
    AccountKey: string,
    Password: string,
    ConfirmPassword: string
  ): Observable<{ success: boolean }> {
    //URL
    const baseURL: string = 'http://localhost:2020';

    console.log(this.generateDeviceKey());
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
      ConfirmPassword,
    };

    return this.httpClient
      .post<{ success: boolean; expiresAt: string; token: string }>(
        baseURL + '/api/account/signup',
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
