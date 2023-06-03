import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ColorUsedService } from '../../services/color-used.service';
import { NgForm } from '@angular/forms';
import { ClipBoardComponent } from 'src/app/clipBoard/clipBoard.component';
import { IClipBoard } from 'src/app/clipBoard/IClipBoard';
import { fromEvent, map, tap } from 'rxjs';
import { MatDialogRef } from '@angular/material/dialog';
import { MobileViewService } from '../../services/mobile-view.service';

@Component({
  selector: 'app-add-to-clipboard-modal',
  templateUrl: './add-to-clipboard-modal.component.html',
  styleUrls: ['./add-to-clipboard-modal.component.css'],
})
export class AddToClipboardModalComponent implements OnInit {
  constructor(
    private colorUsed: ColorUsedService,
    private clipBoardComponent: ClipBoardComponent,
    private mobileViewService: MobileViewService,
    private dialogRef: MatDialogRef<AddToClipboardModalComponent> // Inject MatDialogRef
  ) {}

  @ViewChild('AddToClipboardButton')
  AddToClipboardElementRef: ElementRef;

  isBigWidth: boolean;
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

  /*   onEnterPress(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.close();
    }
  } */
  onAddClipBoardList(content: NgForm) {
    this.clipBoardComponent.onAddClipBoardList(content);
    content.form.reset();
    this.close();
  }

  ngAfterViewInit(): void {
    const AddToClipboardButtonElement =
      this.AddToClipboardElementRef.nativeElement;

    if (window.innerWidth < 500) {
      this.mobileViewService.resizeEvent(
        AddToClipboardButtonElement,
        'flex-fill'
      );
    }

    fromEvent(window, 'resize').subscribe(() => {
      if (window.innerWidth < 500) {
        this.mobileViewService.resizeEvent(
          AddToClipboardButtonElement,
          'flex-fill'
        );

        this.isBigWidth = false;
      } else {
        this.isBigWidth = true;
      }
    });
  }
}
