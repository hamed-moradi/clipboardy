import { Component, OnInit, Input } from '@angular/core';
import { ColorUsedService } from 'src/app/help/color-used.service';
import { ClipBoard } from '../../clipBoard.model';

@Component({
  selector: 'app-clipBoard-item',
  templateUrl: './clipBoard-item.component.html',
  styleUrls: ['./clipBoard-item.component.css'],
})
export class ClipBoardItemComponent implements OnInit {
  @Input() clipBoard: ClipBoard;

  constructor(private colorUsedService: ColorUsedService) {}

  babyPowder: string = this.colorUsedService.BabyPowder;
  tiffanyBlue: string = this.colorUsedService.TiffanyBlue;
  orangePeel: string = this.colorUsedService.OrangePeel;
  richBlack: string = this.colorUsedService.RichBlack;
  roseMadder: string = this.colorUsedService.RoseMadder;

  ngOnInit(): void {}
  onClickCopyToClipBoard(event: Event) {
    // Get the text field
    if (event != null) {
      console.log(event);
      const copyText = this.clipBoard.text;

      // Copy the text inside the text field
      navigator.clipboard.writeText(copyText);

      // Alert the copied text
      //alert('Copied the text: ' + copyText);
      console.log(copyText);
    }
  }

  onClickEditClipBoard(event: Event) {}

  onClickDeleteClipBoard() {}
}
