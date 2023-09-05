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
import { filter, fromEvent, map, takeWhile, tap, timeout } from 'rxjs';

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
    this.clipBoardService.getClipBoard(this.skip, this.take).pipe(
      tap((get) => {
        console.log(get);
        this.totalCount = Math.ceil(get.totalCount);
      })
    );
  }

  violet: string = this.colorUsedService.violet;
  pink: string = this.colorUsedService.pink;
  orange: string = this.colorUsedService.orange;
  blue: string = this.colorUsedService.blue;
  green: string = this.colorUsedService.green;

  isSearched: boolean = false;
  isLoading: boolean = false;

  totalCount: number | null = null; // Initialize totalCount as null
  skip: number = 0;
  take: number = 0;

  ngOnInit(): void {
    this.onScrollDown();
  }
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
    const dialogRef = this.dialog.open(AddOrEditClipboardComponent, {
      scrollStrategy: new NoopScrollStrategy(),
    });

    /*   dialogRef.updatePosition({
      right: "500px", // Set right position
      top: "600px", // Set top position
    }); */
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
      //console.log(this.clipBoards);
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

  onScrollDown() {
    if (!this.isLoading) {
      this.isLoading = true;
      this.take += 10;

      if (this.totalCount === null || this.totalCount >= this.take) {
        this.clipBoardService
          .getClipBoard(this.skip, this.take)
          .pipe(
            tap((get) => {
              console.log(get);
              this.totalCount = Math.ceil(get.totalCount);
            })
          )
          .subscribe({
            next: (res) => {
              this.clipBoards = res.list;
              this.isLoading = false; // Move isLoading inside the next callback
            },
          });
      } else {
        this.isLoading = false; // Set isLoading to false if the condition is not met
      }
    }
  }
}
