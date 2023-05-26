import { Component, ErrorHandler } from '@angular/core';
import { AuthService } from '../../../auth/auth.service';
import { SignUpService } from '../../services/sign-up.service';
import { ColorUsedService } from '../../../shared/services/color-used.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { SignInService } from '../../services/sign-in.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
})
export class ModalComponent {
  constructor(
    private colorUsed: ColorUsedService,
    private authService: AuthService,
    private signUpService: SignUpService,
    private signInService: SignInService
  ) {}
  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  //SignUp Method
  onSignUpForm(SignUpuserForm: NgForm) {
    if (SignUpuserForm.valid) {
      this.signUpService
        .signUp(
          SignUpuserForm.value.usernameInput,
          SignUpuserForm.value.passwordInput,
          SignUpuserForm.value.confirmPasswordInput
        )
        .subscribe({
          // handle successful sign-up response
          next: (response) => console.log(response),
          // handle sign-up error
          error: (e) => console.error(e),
        });
    }
  }

  onForgotPassword(ForgotPasswordForm: NgForm) {
    if (ForgotPasswordForm.valid) {
      console.log(ForgotPasswordForm);
      console.log(ForgotPasswordForm.value);
    }
  }
}
