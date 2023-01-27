import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ClipBoardService } from '../clip-Board.service';
import { ClipBoard } from '../clipBoard.model';
import { ColorUsedService } from 'src/app/help/color-used.service';

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
    new ClipBoard('END'),
  ];

  constructor(
    private clipBoardService: ClipBoardService,
    private colorUsedService: ColorUsedService
  ) {}

  babyPowder: string = this.colorUsedService.BabyPowder;
  tiffanyBlue: string = this.colorUsedService.TiffanyBlue;
  orangePeel: string = this.colorUsedService.OrangePeel;
  richBlack: string = this.colorUsedService.RichBlack;
  roseMadder: string = this.colorUsedService.RoseMadder;

  isSearched: boolean = false;

  clipBoards2 = this.clipBoardService.getClipBoard().subscribe((r) => {
    console.log(r + 'get');
  });

  ngOnInit(): void {}

  AddClipBoardList(clipBoardInput: NgForm) {
    const value = clipBoardInput.value.clipBoardInput;
    console.log(value);
    this.clipBoards.push(new ClipBoard(value));

    this.clipBoardService.postClipBoard(value).subscribe((r) => {
      console.log(r);
    });
    clipBoardInput.form.reset();
  }

  ClipBoardSerachList(searchInput: NgForm) {
    //console.log(searchInput);
    if (
      searchInput.value.searchInput === undefined ||
      searchInput.value.searchInput === ''
    ) {
      //console.log(this.clipBoards);
      return this.clipBoards;
    } else {
      var clipBoardFilterd = this.clipBoards.filter((clipBoard) => {
        return clipBoard.text
          .toLowerCase()
          .includes(searchInput.value.searchInput.toLowerCase());
      });

      return clipBoardFilterd;
    }
  }

  onClickAddModal() {}
}
