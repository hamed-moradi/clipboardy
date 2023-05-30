import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth/auth.service';
import { ColorUsedService } from 'src/app/shared/services/color-used.service';
import { SignInService } from 'src/app/shared/services/sign-in.service';
import { SignUpModalComponent } from '../../modal/sign-up-modal/sign-up-modal.component';
import { ForgotPasswordModalComponent } from '../../modal/forgot-password-modal/forgot-password-modal.component';
import { MessagesService } from 'src/app/shared/services/messages.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-sign-in-modal',
  templateUrl: './sign-in-modal.component.html',
  styleUrls: ['./sign-in-modal.component.css'],
})
export class SignInModalComponent {
  constructor(
    private colorUsed: ColorUsedService,
    private authService: AuthService,
    private signInService: SignInService,
    private messages: MessagesService,
    private matSnackBar: MatSnackBar,
    private signUpDialog: MatDialog,
    private forgotPasswordDialog: MatDialog,
    private signIndialogRef: MatDialogRef<SignInModalComponent> // Inject MatDialogRef
  ) {}
  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  fillAlert: void;
  minLenghtMessage: string = this.messages.lengthInfoMessage;
  //SignIn Method
  onSignInForm(SignInuserForm: NgForm) {
    console.log(SignInuserForm.form.valid);
    console.log(SignInuserForm.form.controls);
    if (SignInuserForm.form.valid) {
      console.log(this.authService.isLoggedIn);
      this.signInService
        .signIn(
          SignInuserForm.value.usernameInput,
          SignInuserForm.value.passwordInput,
          SignInuserForm.value.rememberMe
        )
        .subscribe({
          // handle successful sign-up response
          next: (response) => console.log(response),
          // handle sign-up error
          error: (e) => console.error(e),
        });
      this.closeSignInDialog();
    } else {
      // Display error message and highlight invalid input field
      this.fillAlert = alert(this.messages.fillAllFieldsMessage);
    }
  }

  closeSignInDialog() {
    this.signIndialogRef.close(); // Close the dialog
  }

  onEnterPress(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.closeSignInDialog();
    }
  }

  openSignUpDialog() {
    this.signUpDialog.open(SignUpModalComponent);
  }

  openForgotPasswordDialog() {
    console.log('forgot password func');
    this.forgotPasswordDialog.open(ForgotPasswordModalComponent);
  }

  showMessage(message: string) {
    this.matSnackBar.open(message, 'Close', {
      duration: 2000, // Duration for the popup to be visible (in milliseconds)
      horizontalPosition: 'center', // Position of the popup horizontally
      verticalPosition: 'top', // Position of the popup vertically
    });
  }
}
