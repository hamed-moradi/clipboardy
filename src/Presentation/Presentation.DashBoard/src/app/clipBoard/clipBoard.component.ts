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
import { DOCUMENT, IMAGE_CONFIG } from '@angular/common';
import { NgForm } from '@angular/forms';
import { BehaviorSubject, fromEvent, map, Observable, of } from 'rxjs';
import { tap, take, takeWhile } from 'rxjs/operators';

import { ClipBoardService } from '../shared/services/clipBoard.service';
import { IClipBoard } from './IClipBoard';
import { ColorUsedService } from '../shared/services/color-used.service';
import { MobileViewService } from '../shared/services/mobile-view.service';
import { MatDialog } from '@angular/material/dialog';
import { AddToClipboardModalComponent } from '../shared/modals/add-to-clipboard-modal/add-to-clipboard-modal.component';

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
    public clipBoardService: ClipBoardService,
    private colorUsedService: ColorUsedService,
    private mobileViewService: MobileViewService,
    public dialog: MatDialog
  ) {}

  violet: string = this.colorUsedService.violet;
  pink: string = this.colorUsedService.pink;
  orange: string = this.colorUsedService.orange;
  blue: string = this.colorUsedService.blue;
  green: string = this.colorUsedService.green;

  isSearched: boolean = false;

  /*  dataTest$: Observable<IClipBoard[]> = of([
    {
      content:
        'The idea of a body so big that even light could not escape was briefly proposed by English astronomical pioneer and clergyman John Michell in a letter published in November 1784. Michell s simplistic calculations assumed such a body might have the same density as the Sun, and concluded that one would form when a star s diameter exceeds the Sun s by a factor of 500, and its surface escape velocity exceeds the usual speed of light. Michell referred to these bodies as dark stars.[13] He correctly noted that such supermassive but non-radiating bodies might be detectable through their gravitational effects on nearby visible bodies.[8][14][15] Scholars of the time were initially excited by the proposal that giant but invisible "dark stars" might be hiding in plain view, but enthusiasm dampened when the wavelike nature of light became apparent in the early nineteenth century,[16] as if light were a wave rather than a particle, it was unclear what, if any, influence gravity would have on escaping light waves.[8][15]',
    },

    {
      content:
        'One common means to help one convey information and the audience stay on track is through the incorporation of text in a legible font size and type.[9] According to the article "Prepare and Deliver an Effective Presentation",[10] effective presentations typically use serif fonts (e.g. Times New Roman, Garamond, Baskerville, etc.) for the smaller text and sans serif fonts (e.g. Helvetica, Futura, Arial, etc.) for headings and larger text. The typefaces are used along with type size to improve readability for the audience. A combination of these typefaces can also be used to create emphasis. The majority of the fonts within a presentation are kept simple to aid in readability. Font styles, like bold, italic, and underline, are used to highlight important points. It is possible to emphasize text and still maintain its readability by using contrasting colors. For example, black words on a white background emphasize the text being displayed but still helps maintain its readability.[11] Text that contrasts with the background of a slide also enhances visibility. Readability and visibility enhance a presentation experience, which contributes to the effectiveness of it.[citation needed] Certain colors are also associated with specific emotions and the proper application of these colors adds to the effectiveness of a presentation through the creation of an immersive experience for an audience.',
    },
  ]); */

  ngOnInit(): void {
    this.clipBoardService
      .getClipBoard()
      .pipe(map((get) => get.list))
      .subscribe(
        (getClipBoardResult) => (this.clipBoards = getClipBoardResult)
      );
    //************OLD*********** */
    /*  this.dataTest
      .pipe(
        map((list) => list),

        tap((r) => console.log(r))
      )
      .subscribe({
        next: (getClipBoardResult) => (this.clipBoards = getClipBoardResult),
      });
    this.onclipBoards();
  }
*/
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
    this.dialog.open(AddToClipboardModalComponent);
  }

  /*  onclipBoards() {
    this.dataTest$
      .pipe(tap((r) => console.log(r)))
      .subscribe((getClipBoardResult) => {
        this.clipBoards = getClipBoardResult;
      });
  } */

  // Add Clipboard to list
  onAddClipBoardList(content: NgForm, clipBoards: IClipBoard[]) {
    var newContent = content.value;

    this.clipBoardService.AddToClipBoard(newContent).subscribe();
  }

  // Search method
  onClipBoardSearchList(searchQuery: NgForm) {
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
