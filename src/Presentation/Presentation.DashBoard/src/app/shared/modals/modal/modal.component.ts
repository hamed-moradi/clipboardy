import { Component } from '@angular/core';
import { AuthService } from '../../../auth/auth.service';
import { ColorUsedService } from '../../../shared/services/color-used.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
})
export class ModalComponent {
  constructor(
    private colorUsed: ColorUsedService,
    private authService: AuthService
  ) {}
  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  hasToken: boolean;
  onSignInForm(SignInuserForm: NgForm) {
    console.log(SignInuserForm.valid);
    if (SignInuserForm.valid) {
      console.log(SignInuserForm.value);
      this.authService.login();
      console.log(
        'modal SignIn' + ' ' + this.authService.isLoggedIn.getValue()
      );
    }
  }

  onSignUpForm(SignUpuserForm: NgForm) {
    if (SignUpuserForm.valid) {
      console.log(SignUpuserForm);
      console.log(SignUpuserForm.value);
    }
  }

  onForgotPassword(ForgotPasswordForm: NgForm) {
    if (ForgotPasswordForm.valid) {
      console.log(ForgotPasswordForm);
      console.log(ForgotPasswordForm.value);
    }
  }
}
