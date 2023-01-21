import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ClipBoardService } from '../clip-board.service';
import { ClipBoard } from '../clipBoard.model';

@Component({
  selector: 'app-clipBoard-list',
  templateUrl: './clipBoard-list.component.html',
  styleUrls: ['./clipBoard-list.component.css'],
})
export class ClipBoardListComponent implements OnInit {
  clipBoards: ClipBoard[] = [
    new ClipBoard('Test clipBoard 1'),
    new ClipBoard('Test clipBoard 2'),
    new ClipBoard('Test clipBoard 3'),
    new ClipBoard('Test clipBoard 1'),
    new ClipBoard('Test clipBoard 2'),
    new ClipBoard('Test clipBoard 3'),
    new ClipBoard('Test clipBoard 1'),
    new ClipBoard('Test clipBoard 2'),
    new ClipBoard('Test clipBoard 3'),
    new ClipBoard('Test clipBoard 1'),
    new ClipBoard('Test clipBoard 2'),
    new ClipBoard('Test clipBoard 3'),
  ];

  constructor(private clipBoardService: ClipBoardService) {}

  clipBoards2 = this.clipBoardService.getClipBoard().subscribe((r) => {
    console.log(r + 'get');
  });

  ngOnInit(): void {}

  AddClipBoardList(clipBoardInput: NgForm) {
    const value = clipBoardInput.value.inputClipBoard;
    this.clipBoards.push(new ClipBoard(value));

    this.clipBoardService.postClipBoard(value).subscribe((r) => {
      console.log(r);
    });
    clipBoardInput.form.reset();
  }
}