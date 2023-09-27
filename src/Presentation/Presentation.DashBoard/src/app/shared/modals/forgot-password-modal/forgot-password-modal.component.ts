import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { AuthService } from "src/app/shared/services/auth.service";
import { ColorUsedService } from "src/app/shared/services/color-used.service";
import { SignUpService } from "src/app/shared/services/sign-up.service";
import { MessagesService } from "../../services/messages.service";
import Swal from "sweetalert2";

@Component({
  selector: "app-forgot-password-modal",
  templateUrl: "./forgot-password-modal.component.html",
  styleUrls: ["./forgot-password-modal.component.scss"],
})
export class ForgotPasswordModalComponent {
  constructor(
    private colorUsed: ColorUsedService,
    private authService: AuthService,
    private signUpService: SignUpService,
    private messageService: MessagesService,
    private forgotPassworddialogRef: MatDialogRef<ForgotPasswordModalComponent> // Inject MatDialogRef
  ) {}

  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  minLenghtMessage = this.messageService.lengthInfoMessage;
  onForgotPassword(ForgotPasswordForm: NgForm) {
    console.log("forgot work!");
    if (ForgotPasswordForm.valid) {
      //console.log(ForgotPasswordForm);
      //console.log(ForgotPasswordForm.value);
    } else {
      Swal.fire({
        title: "attention!",
        text: this.messageService.fillAllFieldsMessage,
        icon: "warning",
        confirmButtonColor: this.violet,
      });
    }
  }

  closeForgotPassworddialog() {
    this.forgotPassworddialogRef.close(); // Close the dialog
  }
}
