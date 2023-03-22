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
import { DOCUMENT } from '@angular/common';
import { NgForm } from '@angular/forms';
import { BehaviorSubject, fromEvent, map, Observable, of } from 'rxjs';
import { tap, take, takeWhile } from 'rxjs/operators';

import { ClipBoardService } from './clipBoard.service';
import { IClipBoard } from './clipBoard.model';
import { ColorUsedService } from '../help/color-used.service';
import { MobileViewService } from '../shared/services/mobile-view.service';

@Component({
  selector: 'app-clipBoard',
  templateUrl: './clipBoard.component.html',
  styleUrls: ['./clipBoard.component.css'],
})
export class ClipBoardComponent implements OnInit, AfterViewInit {
  clipBoards: IClipBoard[];
  finished: boolean = true;

  @ViewChild('newButtonClipBoard') myElementRef: ElementRef;

  constructor(
    private clipBoardService: ClipBoardService,
    private colorUsedService: ColorUsedService,
    private mobileViewService: MobileViewService
  ) {}

  violet: string = this.colorUsedService.violet;
  pink: string = this.colorUsedService.pink;
  orange: string = this.colorUsedService.orange;
  blue: string = this.colorUsedService.blue;
  green: string = this.colorUsedService.green;

  isSearched: boolean = false;

  dataTest$: Observable<IClipBoard[]> = of([
    {
      content:
        'Angular is running in development mode. Call enableProdMode() to enable production mode.',
    },

    {
      content: 'test Amir',
    },
  ]);

  ngOnInit(): void {
    /*  this.clipBoardService
      .getClipBoard()
      .pipe(
        map((get) => get.list),
        tap((r) => console.log(r))
      )
      .subscribe(
        (getClipBoardResult) => (this.clipBoards = getClipBoardResult)
      ); */

    //************OLD*********** */
    /*  this.dataTest
      .pipe(
        map((list) => list),

        tap((r) => console.log(r))
      )
      .subscribe({
        next: (getClipBoardResult) => (this.clipBoards = getClipBoardResult),
      }); */

    this.onclipBoards();
  }

  ngAfterViewInit(): void {
    if (window.innerWidth < 500) {
      const newButtonClipBoardElement = this.myElementRef.nativeElement;
      console.log(newButtonClipBoardElement);
      this.mobileViewService.resizeEvent(newButtonClipBoardElement, 'd-grid');
    }
    fromEvent(window, 'resize').subscribe(() => {
      if (window.innerWidth < 500) {
        const newButtonClipBoardElement = this.myElementRef.nativeElement;
        console.log(newButtonClipBoardElement);
        this.mobileViewService.resizeEvent(newButtonClipBoardElement, 'd-grid');
      }
    });
  }

  onclipBoards() {
    this.dataTest$
      .pipe(tap((r) => console.log(r)))
      .subscribe((getClipBoardResult) => {
        this.clipBoards = getClipBoardResult;
      });
  }

  // Add Clipboard to list
  onAddClipBoardList(content: NgForm) {
    let newContent = content.value;
    this.clipBoards.push(newContent);

    /*this.clipBoardService.postClipBoard(content).subscribe((r) => {
      console.log(r);
    });
    */
    content.form.reset();
    console.log(this.clipBoards);
  }

  // Search method
  onClipBoardSerachList(searchQuery: NgForm) {
    if (
      searchQuery.value.searchQuery === undefined ||
      searchQuery.value.searchQuery === ''
    ) {
      return this.clipBoards;
    } else {
      var clipBoardFilterd = this.clipBoards.filter((clipBoard) => {
        return clipBoard.content
          .toLowerCase()
          .includes(searchQuery.value.searchQuery.toLowerCase());
      });
      return clipBoardFilterd;
    }
  }

  onScroll() {}
}
