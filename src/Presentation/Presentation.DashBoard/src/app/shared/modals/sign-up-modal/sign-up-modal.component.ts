import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { AuthService } from "src/app/shared/services/auth.service";
import { ColorUsedService } from "src/app/shared/services/color-used.service";
import { SignUpService } from "src/app/shared/services/sign-up.service";
import { MessagesService } from "../../services/messages.service";
import Swal from "sweetalert2";

@Component({
  selector: "app-sign-up-modal",
  templateUrl: "./sign-up-modal.component.html",
  styleUrls: ["./sign-up-modal.component.scss"],
})
export class SignUpModalComponent {
  constructor(
    private colorUsed: ColorUsedService,
    private authService: AuthService,
    private signUpService: SignUpService,
    private messageService: MessagesService,
    private signUpdialogRef: MatDialogRef<SignUpModalComponent> // Inject MatDialogRef
  ) {}

  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  minLenghtMessage = this.messageService.lengthInfoMessage;
  //SignUp Method
  onSignUpForm(SignUpuserForm: NgForm) {
    if (SignUpuserForm.valid) {
      // console.log("signUp work!");
      this.signUpService
        .signUp(
          SignUpuserForm.value.usernameInput,
          SignUpuserForm.value.passwordInput,
          SignUpuserForm.value.confirmPasswordInput
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

  closeSignUpDialog() {
    this.signUpdialogRef.close();
  }
}
