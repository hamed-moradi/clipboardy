import { Component, OnInit, Input } from '@angular/core';
import { ColorUsedService } from 'src/app/help/color-used.service';
import { IClipBoard } from '../clipBoard.model';

@Component({
  selector: 'app-clipBoard-item',
  templateUrl: './clipBoard-item.component.html',
  styleUrls: ['./clipBoard-item.component.css'],
})
export class ClipBoardItemComponent implements OnInit {
  @Input() clipBoard: IClipBoard;

  constructor(private colorUsedService: ColorUsedService) {}

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
