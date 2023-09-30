import { Component, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { AuthService } from "src/app/shared/services/auth.service";
import { ColorUsedService } from "src/app/shared/services/color-used.service";
import { MessagesService } from "src/app/shared/services/messages.service";
import Swal from "sweetalert2";
import { ResetPasswordService } from "../services/reset-password.service";

@Component({
  selector: "app-reset-password",
  templateUrl: "./reset-password.component.html",
  styleUrls: ["./reset-password.component.scss"],
})
export class ResetPasswordComponent implements OnInit {
  constructor(
    private colorUsed: ColorUsedService,
    private authService: AuthService,
    private resetPasswordService: ResetPasswordService,
    private messageService: MessagesService,
    private resetPasswordDialogRef: MatDialogRef<ResetPasswordComponent> // Inject MatDialogRef
  ) {}

  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;
  white: string = this.colorUsed.white;
  black: string = this.colorUsed.black;

  minLenghtMessage = this.messageService.lengthInfoMessage;

  ngOnInit(): void {
    const hero = document.querySelector(".hero") as HTMLElement | null;

    const changeTheme = document.querySelector(
      ".changetheme"
    ) as HTMLElement | null;

    //in dark mode
    if (localStorage.getItem("isDarkMode") == "true") {
      if (hero) {
        hero.style.backgroundImage = 'url("assets/img/theme/dark-home.jpg")';
      }
      if (changeTheme) {
        changeTheme.style.color = this.white;
      }

      /*  if (ImgContactUS) {
        ImgContactUS.setAttribute("src", "assets/img/theme/contactUS.png");
      } */

      // in light mode
    } else {
      if (hero) {
        hero.style.removeProperty("background-image");
        hero.style.removeProperty("background");
      }

      if (changeTheme) {
        changeTheme.style.color = this.black;
      }
    }
  }
  //resetPassword Method
  onResetPasswordForm(resetPasswordUserForm: NgForm) {
    if (resetPasswordUserForm.valid) {
      // console.log("resetPassword work!");
      this.resetPasswordService
        .resetPassword(
          resetPasswordUserForm.value.usernameInput,
          resetPasswordUserForm.value.passwordInput,
          resetPasswordUserForm.value.confirmPasswordInput
        )
        .subscribe({
          // handle successful resetPassword response
          // next: (response) => console.log(response),
          // handle resetPassword error
          error: (errMes) => {
            //console.error(errMes),
            Swal.fire({
              title: "Error!",
              text: errMes.error.value,
              icon: "error",
              confirmButtonColor: this.violet,
            });
          },
        });
    } else {
      Swal.fire({
        title: "attention!",
        text: this.messageService.fillAllFieldsMessage,
        icon: "warning",
        confirmButtonColor: this.violet,
      });
    }
  }

  closeResetPasswordDialog() {
    this.resetPasswordDialogRef.close();
  }
}
