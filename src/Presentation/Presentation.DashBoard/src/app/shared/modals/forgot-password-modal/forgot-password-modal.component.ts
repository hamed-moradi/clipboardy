import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { ForgotPasswordService } from "src/app/shared/services/forgot-password.service";

import { ColorUsedService } from "src/app/shared/services/color-used.service";
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
    private ForgotPasswordService: ForgotPasswordService,
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
    if (ForgotPasswordForm.valid) {
      this.ForgotPasswordService.forgotPassword(
        ForgotPasswordForm.value.emailInput
      ).subscribe({
        next: () => {
          Swal.fire(
            "An url sent to your email  ",
            "Please check your email for reset password.",
            "success"
          );
        },
        error: (ErrMsg) => {
          Swal.fire({
            title: "error",
            text: ErrMsg.error.value,
            icon: "error",
            confirmButtonColor: this.violet,
          });
        },
      });
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
