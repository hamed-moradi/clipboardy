import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  Inject,
  ViewChild,
  ElementRef,
  AfterViewInit,
} from '@angular/core';
import { NgForm } from '@angular/forms';
import { fromEvent, map } from 'rxjs';

import { ClipBoardService } from '../shared/services/clipBoard.service';
import { IClipBoard } from './IClipBoard';
import { ColorUsedService } from '../shared/services/color-used.service';
import { MobileViewService } from '../shared/services/mobile-view.service';
import { MatDialog } from '@angular/material/dialog';
import { AddOrEditClipboardComponent } from '../shared/modals/addOrEditClipboard-modal/addOrEditClipboard-modal.component';
import { Router } from '@angular/router';
import { ErrorModalComponent } from '../shared/modals/error-modal/error-modal.component';
import { NoopScrollStrategy } from '@angular/cdk/overlay';

@Component({
  selector: 'app-clipBoard',
  templateUrl: './clipBoard.component.html',
  styleUrls: ['./clipBoard.component.scss'],
})
export class ClipBoardComponent implements OnInit, AfterViewInit {
  clipBoards: IClipBoard[];

  finished: boolean = true;

  @ViewChild('newButtonClipBoard') myElementRef: ElementRef;

  constructor(
    public clipBoardService: ClipBoardService,
    private colorUsedService: ColorUsedService,
    private mobileViewService: MobileViewService,
    public dialog: MatDialog,
    private errorDialog: MatDialog,
    private router: Router
  ) {
    this.clipBoardService
      .getClipBoard()
      .pipe(map((get) => get.list))
      .subscribe(
        (getClipBoardResult) => (this.clipBoards = getClipBoardResult)
      );
  }

  violet: string = this.colorUsedService.violet;
  pink: string = this.colorUsedService.pink;
  orange: string = this.colorUsedService.orange;
  blue: string = this.colorUsedService.blue;
  green: string = this.colorUsedService.green;

  isSearched: boolean = false;

  ngOnInit(): void {}
  ngAfterViewInit(): void {
    if (window.innerWidth < 500) {
      const newButtonClipBoardElement = this.myElementRef.nativeElement;
      this.mobileViewService.resizeEvent(newButtonClipBoardElement, 'd-grid');
    }
    fromEvent(window, 'resize').subscribe(() => {
      if (window.innerWidth < 500) {
        const newButtonClipBoardElement = this.myElementRef.nativeElement;
        this.mobileViewService.resizeEvent(newButtonClipBoardElement, 'd-grid');
      }
    });
  }

  openAddToClipBoardDialog() {
    this.dialog.open(AddOrEditClipboardComponent, {
      scrollStrategy: new NoopScrollStrategy(),
    });
  }

  // Add Clipboard to list
  onAddClipBoardList(content: NgForm) {
    var newContent = content.value;

    this.clipBoardService.AddToClipBoard(newContent).subscribe({
      // handle successful sign-up response
      next: (response) => {
        console.log(response),
          // Reload the page after adding a new clipboard item
          window.location.reload();
      },
      // handle error
      error: (errMes) => {
        console.error(errMes),
          console.error(errMes.error.title),
          // Show error dialog
          this.errorDialog.open(ErrorModalComponent, {
            data: {
              message: 'An error occurred during Add content to clipboard.',

              error: errMes.error.title,
            },
          });
      },
    });
  }

  // Edit Clipboard
  onUpdateClipBoard(editedContent: NgForm, id: number) {
    var editedContentValue = String(Object.values(editedContent.value)[0]);

    this.clipBoardService.UpdateClipBoard(editedContentValue, id).subscribe({
      // handle successful sign-up response
      next: (response) => {
        console.log(response),
          // Reload the page after adding a new clipboard item

          window.location.reload();
      },
      // handle error
      error: (errMes) => {
        console.error(errMes),
          console.error(errMes.error.title),
          // Show error dialog
          this.errorDialog.open(ErrorModalComponent, {
            data: {
              message: 'An error occurred during Edit content clipboard.',

              error: errMes.error.title,
            },
          });
      },
    });
  }

  // Delete Clipboard
  onDeleteClipBoard(id: number) {
    this.clipBoardService.DeleteClipBoard(id).subscribe({
      // handle successful sign-up response
      next: (response) => {
        console.log(response),
          // Reload the page after adding a new clipboard item

          window.location.reload();
      },
      // handle error
      error: (errMes) => {
        console.error(errMes),
          console.error(errMes.error.title),
          // Show error dialog
          this.errorDialog.open(ErrorModalComponent, {
            data: {
              message: 'An error occurred during Delete content clipboard.',

              error: errMes.error.title,
            },
          });
      },
    });
  }

  // Search method
  onClipBoardSearchList(searchQuery: NgForm) {
    if (
      searchQuery.value.searchQuery === undefined ||
      searchQuery.value.searchQuery === ''
    ) {
      return this.clipBoards;
    } else {
      var clipBoardFilterd = this.clipBoards.filter((clipBoard: IClipBoard) =>
        clipBoard.content
          .toLowerCase()
          .includes(searchQuery.value.searchQuery.toLowerCase())
      );

      return clipBoardFilterd;
    }
  }

  /*   onScroll() {
    console.log('scrolled');
  } */
}
