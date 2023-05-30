import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth/auth.service';
import { ColorUsedService } from 'src/app/shared/services/color-used.service';
import { SignUpService } from 'src/app/shared/services/sign-up.service';

@Component({
  selector: 'app-forgot-password-modal',
  templateUrl: './forgot-password-modal.component.html',
  styleUrls: ['./forgot-password-modal.component.css'],
})
export class ForgotPasswordModalComponent {
  constructor(
    private colorUsed: ColorUsedService,
    private authService: AuthService,
    private signUpService: SignUpService,
    private dialogRef: MatDialogRef<ForgotPasswordModalComponent> // Inject MatDialogRef
  ) {}

  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  onForgotPassword(ForgotPasswordForm: NgForm) {
    if (ForgotPasswordForm.valid) {
      console.log(ForgotPasswordForm);
      console.log(ForgotPasswordForm.value);
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
