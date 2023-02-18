import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ClipBoardService } from './clipBoard.service';
import { ClipBoard } from './clipBoard.model';
import { ColorUsedService } from '../help/color-used.service';

@Component({
  selector: 'app-clipBoard',
  templateUrl: './clipBoard.component.html',
  styleUrls: ['./clipBoard.component.css'],
})
export class ClipBoardComponent implements OnInit {
  clipBoards: ClipBoard[] = [new ClipBoard('Test clipBoard 1')];

  constructor(
    private clipBoardService: ClipBoardService,
    private colorUsedService: ColorUsedService
  ) {}

  violet: string = this.colorUsedService.violet;
  pink: string = this.colorUsedService.pink;
  orange: string = this.colorUsedService.orange;
  blue: string = this.colorUsedService.blue;
  green: string = this.colorUsedService.green;

  isSearched: boolean = false;

  clipBoards2 = this.clipBoardService.getClipBoard().subscribe((r) => {
    console.log(r + 'get');
  });

  ngOnInit(): void {
    var getClipBoard = this.clipBoardService.getClipBoard().subscribe();
    console.log(getClipBoard);
    //this.clipBoards.push(getClipBoard);
  }

  onAddClipBoardList(clipBoardInput: NgForm) {
    const content = clipBoardInput.value.clipBoardInput;
    console.log(content);
    this.clipBoards.push(new ClipBoard(content));

    this.clipBoardService.postClipBoard(content).subscribe((r) => {
      console.log(r);
    });
    clipBoardInput.form.reset();
  }

  onClipBoardSerachList(searchInput: NgForm) {
    //console.log(searchInput);
    if (
      searchInput.value.searchInput === undefined ||
      searchInput.value.searchInput === ''
    ) {
      //console.log(this.clipBoards);
      return this.clipBoards;
    } else {
      var clipBoardFilterd = this.clipBoards.filter((clipBoard) => {
        return clipBoard.content
          .toLowerCase()
          .includes(searchInput.value.searchInput.toLowerCase());
      });

      return clipBoardFilterd;
    }
  }
}
