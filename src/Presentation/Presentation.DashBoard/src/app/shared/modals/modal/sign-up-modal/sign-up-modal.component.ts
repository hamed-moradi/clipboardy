import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth/auth.service';
import { ColorUsedService } from 'src/app/shared/services/color-used.service';
import { SignUpService } from 'src/app/shared/services/sign-up.service';

@Component({
  selector: 'app-sign-up-modal',
  templateUrl: './sign-up-modal.component.html',
  styleUrls: ['./sign-up-modal.component.css'],
})
export class SignUpModalComponent {
  constructor(
    private colorUsed: ColorUsedService,
    private authService: AuthService,
    private signUpService: SignUpService,
    private dialogRef: MatDialogRef<SignUpModalComponent> // Inject MatDialogRef
  ) {}
  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;
}
