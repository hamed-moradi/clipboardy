import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, tap } from "rxjs";
import { IUser } from "src/app/auth/IUser";

@Injectable({
  providedIn: "root",
})
export class ForgotPasswordService {
  constructor(private httpClient: HttpClient) {}

  forgotPassword(AccountKey: string): Observable<{ success: boolean }> {
    //URL
    const baseURL: string = "http://localhost:2020";

    //body
    const body: IUser = {
      AccountKey,
    };

    return this.httpClient
      .post<{ success: boolean }>(
        baseURL + "/api/account/ForgotPasswordRequested",
        body
      )
      .pipe(
        tap((response) => {
          console.log(response);
        })
      );
  }
}
