import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { IUser } from "src/app/auth/IUser";
import { Observable, tap } from "rxjs";
import { Tab } from "bootstrap";
import { AuthService } from "./auth.service";

@Injectable({
  providedIn: "root",
})
export class SignUpService {
  constructor(
    private httpClient: HttpClient,
    private authService: AuthService
  ) {}

  signUp(
    AccountKey: string,
    Password: string,
    ConfirmPassword: string
  ): Observable<{ success: boolean }> {
    //URL
    const baseURL: string = "http://localhost:2020";

    //body
    const body: IUser = {
      AccountKey,
      Password,
      ConfirmPassword,
    };

    this.authService.login(
      localStorage.getItem("token") || sessionStorage.getItem("token")
    );

    return this.httpClient
      .post<{ success: boolean; expiresAt: string; token: string }>(
        baseURL + "/api/account/signup",
        body
      )
      .pipe(tap((response) => {}));
  }
}
