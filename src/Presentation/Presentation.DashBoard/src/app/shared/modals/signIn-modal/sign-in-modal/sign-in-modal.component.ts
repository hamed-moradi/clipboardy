import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth/auth.service';
import { ColorUsedService } from 'src/app/shared/services/color-used.service';
import { SignInService } from 'src/app/shared/services/sign-in.service';

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
    private dialogRef: MatDialogRef<SignInModalComponent> // Inject MatDialogRef
  ) {}
  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

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
    } else {
      // Display error message and highlight invalid input field
      alert('Please fill all fields');
    }
  }

  close() {
    this.dialogRef.close(); // Close the dialog
  }

  onEnterPress(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.close();
    }
  }
}
