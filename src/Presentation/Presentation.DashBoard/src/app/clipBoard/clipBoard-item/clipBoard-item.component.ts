import {
  Component,
  OnInit,
  Input,
  ViewChild,
  ElementRef,
  AfterViewInit,
} from "@angular/core";
import * as bootstrap from "bootstrap";
import { fromEvent } from "rxjs";
import { debounceTime } from "rxjs/operators";

import { ColorUsedService } from "../../shared/services/color-used.service";
import { MobileViewService } from "src/app/shared/services/mobile-view.service";
import { IClipBoard } from "../IClipBoard";
import { AddOrEditClipboardComponent } from "src/app/shared/modals/addOrEditClipboard-modal/addOrEditClipboard-modal.component";
import { MatDialog } from "@angular/material/dialog";
import { DataSharingService } from "src/app/shared/services/data-sharing.service";
import { ClipBoardService } from "src/app/shared/services/clipBoard.service";

@Component({
  selector: "app-clipBoard-item",
  templateUrl: "./clipBoard-item.component.html",
  styleUrls: ["./clipBoard-item.component.css"],
})
export class ClipBoardItemComponent implements OnInit, AfterViewInit {
  @Input() clipBoard: IClipBoard;

  @ViewChild("ClipboardItemsCopyButton")
  ClipboardItemsCopyButtonElementRef: ElementRef;

  @ViewChild("ClipboardItemsEditButton")
  ClipboardItemsEditButtonElementRef: ElementRef;

  @ViewChild("ClipboardItemsDeleteButton")
  ClipboardItemsDeleteButtonElementRef: ElementRef;

  constructor(
    private colorUsedService: ColorUsedService,
    private mobileViewService: MobileViewService,
    private editDialog: MatDialog,
    private editMOdeService: DataSharingService,
    private clipBoardService: ClipBoardService
  ) {}

  violet: string = this.colorUsedService.violet;
  pink: string = this.colorUsedService.pink;
  lightPink: string = this.colorUsedService.lightPink;
  orange: string = this.colorUsedService.orange;
  blue: string = this.colorUsedService.blue;
  green: string = this.colorUsedService.green;

  isBigContent: boolean = false;
  isEditDialogOpen: boolean = false;

  ngOnInit(): void {
    /*  const tooltipTriggerList = [].slice.call(
      document.querySelectorAll('[data-bs-toggle="tooltip"]')
    );
    const tooltipList = tooltipTriggerList.map(
      (tooltipTriggerEl) => new bootstrap.Tooltip(tooltipTriggerEl)
    ); */

    const clipBoardContent: string = this.clipBoard.content;
    if (clipBoardContent.length > 136) {
      this.isBigContent = true;
    }
  }

  ngAfterViewInit(): void {
    const ClipboardItemsCopyButtonElement =
      this.ClipboardItemsCopyButtonElementRef.nativeElement;
    const ClipboardItemsEditButtonElement =
      this.ClipboardItemsEditButtonElementRef.nativeElement;
    const ClipboardItemsDeleteButtonElement =
      this.ClipboardItemsDeleteButtonElementRef.nativeElement;

    if (window.innerWidth < 500) {
      this.mobileViewService.resizeEvent(
        ClipboardItemsCopyButtonElement,
        "flex-fill"
      );
      this.mobileViewService.resizeEvent(
        ClipboardItemsEditButtonElement,
        "flex-fill"
      );
      this.mobileViewService.resizeEvent(
        ClipboardItemsDeleteButtonElement,
        "flex-fill"
      );
    }

    fromEvent(window, "resize").subscribe(() => {
      if (window.innerWidth < 500) {
        this.mobileViewService.resizeEvent(
          ClipboardItemsCopyButtonElement,
          "flex-fill"
        );
        this.mobileViewService.resizeEvent(
          ClipboardItemsEditButtonElement,
          "flex-fill"
        );
        this.mobileViewService.resizeEvent(
          ClipboardItemsDeleteButtonElement,
          "flex-fill"
        );
      }
    });
  }

  onClickCopyToClipBoard(event: Event) {
    // Get the content field
    if (event != null) {
      //  console.log(event);
      const copyContent = this.clipBoard.content;

      // Copy the content inside the content field
      navigator.clipboard.writeText(copyContent);

      // tooltip copied content
    }
  }

  openEditClipBoardDialog() {
    console.log("open dialog Edit");
    this.editDialog.open(AddOrEditClipboardComponent, {
      data: {
        content: this.clipBoard.content,
        id: this.clipBoard.id,
      },
    });

    this.editMOdeService.setIsEditMode(true);
    this.editDialog.afterAllClosed.subscribe(() => {
      this.editMOdeService.setIsEditMode(false);
    });
  }

  // Delete Clipboard
  onDeleteClipBoard(event: Event) {
    if (event != null) {
      console.log(this.clipBoard.id);
      this.clipBoardService.DeleteClipBoard(this.clipBoard.id).subscribe({
        // handle successful sign-up response
        next: (response) => {
          console.log(response),
            // Reload the page after adding a new clipboard item

            window.location.reload();
        },
      });
    } else {
      alert("خطا");
    }
  }
}
