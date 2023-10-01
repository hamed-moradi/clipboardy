import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { AuthService } from "src/app/shared/services/auth.service";
import { ColorUsedService } from "src/app/shared/services/color-used.service";
import { MessagesService } from "../../services/messages.service";
import Swal from "sweetalert2";
import { ChangePasswordService } from "src/app/shared/services/change-password.service";
@Component({
  selector: "app-change-password-modal",
  templateUrl: "./change-password-modal.component.html",
  styleUrls: ["./change-password-modal.component.scss"],
})
export class ChangePasswordModalComponent {
  constructor(
    private colorUsed: ColorUsedService,
    private authService: AuthService,
    private changePasswordService: ChangePasswordService,
    private messageService: MessagesService,
    private changePasswordDialogRef: MatDialogRef<ChangePasswordModalComponent> // Inject MatDialogRef
  ) {}

  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  minLenghtMessage = this.messageService.lengthInfoMessage;

  //ChangePassword Method
  onChangePasswordForm(ChangePasswordUserForm: NgForm) {
    if (ChangePasswordUserForm.valid) {
      // console.log("changePassword work!");
      this.changePasswordService
        .changePassword(
          ChangePasswordUserForm.value.oldPasswordInput,
          ChangePasswordUserForm.value.passwordInput,
          ChangePasswordUserForm.value.confirmPasswordInput
        )
        .subscribe({
          // handle successful sign-up response
          // next: (response) => console.log(response),
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
    } else {
      Swal.fire({
        title: "attention!",
        text: this.messageService.fillAllFieldsMessage,
        icon: "warning",
        confirmButtonColor: this.violet,
      });
    }
  }

  closeChangePasswordDialog() {
    this.changePasswordDialogRef.close();
  }
}
