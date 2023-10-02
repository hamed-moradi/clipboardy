import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { AuthService } from "src/app/shared/services/auth.service";
import { ColorUsedService } from "src/app/shared/services/color-used.service";
import { SignInService } from "src/app/shared/services/sign-in.service";
import { SignUpModalComponent } from "../sign-up-modal/sign-up-modal.component";
import { ForgotPasswordModalComponent } from "../forgot-password-modal/forgot-password-modal.component";
import { MessagesService } from "src/app/shared/services/messages.service";
import Swal from "sweetalert2";

@Component({
  selector: "app-sign-in-modal",
  templateUrl: "./sign-in-modal.component.html",
  styleUrls: ["./sign-in-modal.component.scss"],
})
export class SignInModalComponent {
  constructor(
    private colorUsed: ColorUsedService,
    private authService: AuthService,
    private signInService: SignInService,
    private messageService: MessagesService,
    private signUpDialog: MatDialog,
    private forgotPasswordDialog: MatDialog,
    private signInDialogRef: MatDialogRef<SignInModalComponent>,
    private signUpDialogRef: MatDialogRef<SignUpModalComponent>
  ) {}
  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  minLenghtMessage: string = this.messageService.lengthInfoMessage;
  //SignIn Method
  onSignInForm(SignInuserForm: NgForm) {
    if (SignInuserForm.form.valid) {
      // console.log(this.authService.isLoggedIn);
      this.signInService
        .signIn(
          SignInuserForm.value.usernameInput,
          SignInuserForm.value.passwordInput,
          Boolean(SignInuserForm.value.rememberMe)
        )
        .subscribe({
          // handle successful sign-up response
          next: (response) => {
            //console.log(response),
            this.authService.login(
              localStorage.getItem("token")! || sessionStorage.getItem("token")!
            );
          },
          // handle sign-up error
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
      this.closeSignInDialog();
    } else {
      // Display error message and highlight invalid input field
      alert(this.messageService.fillAllFieldsMessage);
    }
  }

  closeSignInDialog() {
    this.signInDialogRef.close(); // Close the dialog
  }

  onEnterPress(event: KeyboardEvent) {
    if (event.key === "Enter") {
      this.closeSignInDialog();
    }
  }

  openSignUpDialog() {
    this.signUpDialog.open(SignUpModalComponent);
  }

  closeSignUpDialog() {
    this.signUpDialogRef.close(); // Close the dialog
  }

  openForgotPasswordDialog() {
    this.forgotPasswordDialog.open(ForgotPasswordModalComponent);
  }
}
