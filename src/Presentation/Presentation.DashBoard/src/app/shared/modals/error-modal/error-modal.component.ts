import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
  styleUrls: ['./error-modal.component.css'],
})
export class ErrorModalComponent {
  constructor(
    private dialogRef: MatDialogRef<ErrorModalComponent>, // Inject MatDialogRef
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  close() {
    this.dialogRef.close(); // Close the dialog
  }

  closeErrorDialog() {
    this.close(); // Close the dialog
  }
}
