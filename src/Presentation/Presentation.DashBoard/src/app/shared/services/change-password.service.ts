import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { IUser } from "src/app/auth/IUser";
import { Observable, tap } from "rxjs";
import { Tab } from "bootstrap";
import { AuthService } from "./auth.service";

@Injectable({
  providedIn: "root",
})
export class ChangePasswordService {
  constructor(
    private httpClient: HttpClient,
    private authService: AuthService
  ) {}

  changePassword(
    Password: string,
    NewPassword: string,
    ConfirmPassword: string
  ): Observable<{ success: boolean }> {
    //URL
    const baseURL: string = "http://localhost:2020";

    //body
    const body: IUser = {
      Password,
      NewPassword,
      ConfirmPassword,
    };

    this.authService.login(
      localStorage.getItem("token") || sessionStorage.getItem("token")
    );

    return this.httpClient
      .post<{ success: boolean; expiresAt: string; token: string }>(
        baseURL + "/api/account/changePassword",
        body
      )
      .pipe(tap((response) => {}));
  }
}
