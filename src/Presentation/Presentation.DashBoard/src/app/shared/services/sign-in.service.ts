import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Observable, catchError, tap, throwError } from "rxjs";
import { IUser } from "src/app/auth/IUser";
import { AuthService } from "src/app/shared/services/auth.service";
import { ErrorModalComponent } from "../modals/error-modal/error-modal.component";
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

    // this.authService.login(localStorage.getItem("token"));

    return this.httpClient
      .post<{
        success: boolean;
        expiresAt: string;
        token: string;
        rememberMe: boolean;
      }>(baseURL + "/api/account/signin", body)
      .pipe(
        tap((response) => {
          //store the expiresAt and token values in localStorage
          localStorage.setItem("expiresAt", response.expiresAt);
          localStorage.setItem("token", response.token);

          //store the RemmeberMe values in localStorage
          localStorage.setItem("rememberMe", String(response.rememberMe));
          console.log("set remmeber");
          console.log(response.rememberMe);
        })
        /*      catchError((error) => {
          // Handle and display the error
          console.error(error);

          // Show error dialog
          this.dialog.open(ErrorModalComponent, {
            data: {
              message: 'An error occurred during sign-in.' + error,
            },
          });
        }) */
      );
  }
}
