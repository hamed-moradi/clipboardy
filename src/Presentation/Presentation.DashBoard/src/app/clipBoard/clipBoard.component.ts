import { Component, OnInit } from '@angular/core';
import { ClipBoard } from './clipBoard-list/clipBoard.model';

@Component({
  selector: 'app-clipBoards',
  templateUrl: './clipBoard.component.html',
  styleUrls: ['./clipBoard.component.css']
})
export class ClipBoardComponent implements OnInit {
  selectedclipBoard:ClipBoard;
  constructor() { }

  ngOnInit(): void {
    console.log(this.selectedclipBoard);
  }

}
