import { Component, OnInit } from '@angular/core';
import { ColorUsedService } from '../../services/color-used.service';
import { NgForm } from '@angular/forms';
import { ClipBoardComponent } from 'src/app/clipBoard/clipBoard.component';
import { IClipBoard } from 'src/app/clipBoard/IClipBoard';
import { tap } from 'rxjs';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-to-clipboard-modal',
  templateUrl: './add-to-clipboard-modal.component.html',
  styleUrls: ['./add-to-clipboard-modal.component.css'],
})
export class AddToClipboardModalComponent implements OnInit {
  constructor(
    private colorUsed: ColorUsedService,
    private clipBoardComponent: ClipBoardComponent,
    private dialogRef: MatDialogRef<AddToClipboardModalComponent> // Inject MatDialogRef
  ) {}

  clipBoards: IClipBoard[] = [];

  close() {
    this.dialogRef.close(); // Close the dialog
  }

  ngOnInit() {}
  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  onEnterPress(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.close();
    }
  }
  onAddClipBoardList(content: NgForm) {
    this.clipBoardComponent.dataTest$
      .pipe(tap((r) => console.log(r)))
      .subscribe((getClipBoardResult) => {
        this.clipBoards = getClipBoardResult;
      });

    this.clipBoardComponent.onAddClipBoardList(content, this.clipBoards);
    content.form.reset();
  }
}
