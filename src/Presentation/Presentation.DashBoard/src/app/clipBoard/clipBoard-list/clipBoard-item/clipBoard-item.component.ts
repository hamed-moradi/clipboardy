import { Component, OnInit, Input } from '@angular/core';
import { ClipBoard } from '../../clipBoard.model';

@Component({
  selector: 'app-clipBoard-item',
  templateUrl: './clipBoard-item.component.html',
  styleUrls: ['./clipBoard-item.component.css'],
})
export class ClipBoardItemComponent implements OnInit {
  @Input() clipBoard: ClipBoard;

  constructor() {}

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

  onClickDeleteClipBoard() {}
}
