import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { ColorUsedService } from "../../services/color-used.service";
import { NgForm } from "@angular/forms";
import { ClipBoardComponent } from "src/app/clipBoard/clipBoard.component";
import { IClipBoard } from "src/app/clipBoard/IClipBoard";
import { fromEvent, map, tap } from "rxjs";
import { MatDialogRef } from "@angular/material/dialog";
import { MobileViewService } from "../../services/mobile-view.service";
import { ClipBoardItemComponent } from "src/app/clipBoard/clipBoard-item/clipBoard-item.component";
import { DataSharingService } from "../../services/data-sharing.service";

@Component({
  selector: "addOrEditClipboard-modal",
  templateUrl: "./addOrEditClipboard-modal.component.html",
  styleUrls: ["./addOrEditClipboard-modal.component.css"],
})
export class AddOrEditClipboardComponent implements OnInit {
  constructor(
    private colorUsed: ColorUsedService,
    private clipBoardComponent: ClipBoardComponent,
    private clipBoardItemComponent: ClipBoardItemComponent,
    private editModeService: DataSharingService,
    private mobileViewService: MobileViewService,
    private dialogRef: MatDialogRef<AddOrEditClipboardComponent> // Inject MatDialogRef
  ) {}

  @ViewChild("AddToClipboardButton")
  AddToClipboardElementRef: ElementRef;

  isBigWidth: boolean;
  clipBoards: IClipBoard[] = [];
  isEditMode: boolean;
  clipBoardContent = this.clipBoardItemComponent.clipBoard.content;

  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  close() {
    this.dialogRef.close(); // Close the dialog
  }

  ngOnInit() {
    this.isEditMode = this.editModeService.getIsEditMode();
  }

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

  onEditlipBoard(selectedContent: NgForm) {
    console.log(selectedContent);
  }

  ngAfterViewInit(): void {
    const AddToClipboardButtonElement =
      this.AddToClipboardElementRef.nativeElement;

    if (window.innerWidth < 500) {
      this.mobileViewService.resizeEvent(
        AddToClipboardButtonElement,
        "flex-fill"
      );
    }

    fromEvent(window, "resize").subscribe(() => {
      if (window.innerWidth < 500) {
        this.mobileViewService.resizeEvent(
          AddToClipboardButtonElement,
          "flex-fill"
        );

        this.isBigWidth = false;
      } else {
        this.isBigWidth = true;
      }
    });
  }
}
