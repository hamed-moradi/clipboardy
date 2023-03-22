import {
  Component,
  OnInit,
  Input,
  ViewChild,
  ElementRef,
  AfterViewInit,
} from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

import { ColorUsedService } from 'src/app/help/color-used.service';
import { MobileViewService } from 'src/app/shared/services/mobile-view.service';
import { IClipBoard } from '../clipBoard.model';

@Component({
  selector: 'app-clipBoard-item',
  templateUrl: './clipBoard-item.component.html',
  styleUrls: ['./clipBoard-item.component.css'],
})
export class ClipBoardItemComponent implements OnInit, AfterViewInit {
  @Input() clipBoard: IClipBoard;

  @ViewChild('ClipboardItemsCopyButton')
  ClipboardItemsCopyButtonElementRef: ElementRef;

  @ViewChild('ClipboardItemsEditButton')
  ClipboardItemsEditButtonElementRef: ElementRef;

  @ViewChild('ClipboardItemsDeleteButton')
  ClipboardItemsDeleteButtonElementRef: ElementRef;

  constructor(
    private colorUsedService: ColorUsedService,
    private mobileViewService: MobileViewService
  ) {}

  violet: string = this.colorUsedService.violet;
  pink: string = this.colorUsedService.pink;
  lightPink: string = this.colorUsedService.lightPink;
  orange: string = this.colorUsedService.orange;
  blue: string = this.colorUsedService.blue;
  green: string = this.colorUsedService.green;

  isActiveScroll: boolean = false;

  ngOnInit(): void {
    /* const clipBoardContent: string = this.clipBoard.content;
    if (clipBoardContent.length > 350) {
      this.isActiveScroll = true;
       console.log('lenght' + clipBoardContent.length);
    } */
    //this.mobileViewService.resizeEvent('#button-Items-size', 'd-grid');
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
        'flex-fill'
      );
      this.mobileViewService.resizeEvent(
        ClipboardItemsEditButtonElement,
        'flex-fill'
      );
      this.mobileViewService.resizeEvent(
        ClipboardItemsDeleteButtonElement,
        'flex-fill'
      );
    }

    fromEvent(window, 'resize').subscribe(() => {
      if (window.innerWidth < 500) {
        this.mobileViewService.resizeEvent(
          ClipboardItemsCopyButtonElement,
          'flex-fill'
        );
        this.mobileViewService.resizeEvent(
          ClipboardItemsEditButtonElement,
          'flex-fill'
        );
        this.mobileViewService.resizeEvent(
          ClipboardItemsDeleteButtonElement,
          'flex-fill'
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

      // Alert the copied content
      //alert('Copied the content: ' + copycontent);
      //  console.log(copyContent);
    }
  }

  onClickEditClipBoard(event: Event) {}

  onClickDeleteClipBoard() {}
}