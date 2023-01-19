import { ConstantPool } from '@angular/compiler';
import { Component, OnInit, Input } from '@angular/core';
import { ClipBoard } from '../clipBoard.model';

@Component({
  selector: 'app-clipBoard-item',
  templateUrl: './clipBoard-item.component.html',
  styleUrls: ['./clipBoard-item.component.css'],
})
export class ClipBoardItemComponent implements OnInit {
  @Input() clipBoard: ClipBoard;

  constructor() {}

  ngOnInit(): void {}
  onClickCopyClipBoard() {}
  onClickDeleteClipBoard() {}
}
