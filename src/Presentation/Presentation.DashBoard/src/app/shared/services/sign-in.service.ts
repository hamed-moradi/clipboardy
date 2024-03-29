import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Observable, catchError, tap, throwError } from "rxjs";
import { IUser } from "src/app/auth/IUser";
import { AuthService } from "src/app/shared/services/auth.service";
import { SignInModalComponent } from "../modals/sign-in-modal/sign-in-modal.component";
@Injectable({
  providedIn: "root",
})
export class SignInService {
  constructor(
    private httpClient: HttpClient,
    private authService: AuthService,
    private dialog: MatDialog
  ) {}

  signIn(
    AccountKey: string,
    Password: string,
    RememberMe: boolean
  ): Observable<{ success: boolean }> {
    //URL
    const baseURL: string = "http://localhost:2020";

    //body
    const body: IUser = {
      AccountKey,
      Password,
      RememberMe,
    };

    return this.httpClient
      .post<{
        success: boolean;
        expiresAt: string;
        token: string;
      }>(baseURL + "/api/account/signin", body)
      .pipe(
        tap((response) => {
          if (body.RememberMe) {
            //store the expiresAt and token values in localStorage

            localStorage.setItem("token", response.token);
          } else {
            //store the expiresAt and token values in sessionStorage

            sessionStorage.setItem("token", response.token);
          }
        })
      );
  }
}
