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
import { ColorUsedService } from '../shared/services/color-used.service';
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
        'The idea of a body so big that even light could not escape was briefly proposed by English astronomical pioneer and clergyman John Michell in a letter published in November 1784. Michell s simplistic calculations assumed such a body might have the same density as the Sun, and concluded that one would form when a star s diameter exceeds the Sun s by a factor of 500, and its surface escape velocity exceeds the usual speed of light. Michell referred to these bodies as dark stars.[13] He correctly noted that such supermassive but non-radiating bodies might be detectable through their gravitational effects on nearby visible bodies.[8][14][15] Scholars of the time were initially excited by the proposal that giant but invisible "dark stars" might be hiding in plain view, but enthusiasm dampened when the wavelike nature of light became apparent in the early nineteenth century,[16] as if light were a wave rather than a particle, it was unclear what, if any, influence gravity would have on escaping light waves.[8][15]',
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
      this.mobileViewService.resizeEvent(newButtonClipBoardElement, 'd-grid');
    }
    fromEvent(window, 'resize').subscribe(() => {
      if (window.innerWidth < 500) {
        const newButtonClipBoardElement = this.myElementRef.nativeElement;
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
