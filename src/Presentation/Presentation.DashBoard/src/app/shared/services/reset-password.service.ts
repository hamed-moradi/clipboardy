import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "./auth.service";
import { Observable, tap } from "rxjs";
import { IUser } from "src/app/auth/IUser";

@Injectable({
  providedIn: "root",
})
export class ResetPasswordService {
  constructor(
    private httpClient: HttpClient,
    private authService: AuthService
  ) {}

  resetPassword(
    Password: string,
    ConfirmPassword: string,
    resetPassToken: string
  ): Observable<{ success: boolean }> {
    //URL
    const baseURL: string = "http://localhost:2020";

    //body
    const body: IUser = {
      Password,
      ConfirmPassword,
      resetPassToken,
    };

    /*    this.authService.login(
      localStorage.getItem("token") || sessionStorage.getItem("token")
    ); */

    return this.httpClient
      .post<{ success: boolean }>(baseURL + "/api/account/resetPassword", body)
      .pipe(
        tap((response) => {
          console.log(response);
        })
      );
  }
}
