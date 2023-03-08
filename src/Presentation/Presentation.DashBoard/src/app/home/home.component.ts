import { ConstantPool } from '@angular/compiler';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, NavigationEnd } from '@angular/router';
import { NgwWowService } from 'ngx-wow';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { IClipBoard } from '../clipBoard/clipBoard.model';
import { ColorUsedService } from '../help/color-used.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit, OnDestroy {
  clipBoards: IClipBoard[];

  private wowSubscription: Subscription;

  constructor(
    private router: Router,
    private colorUsedService: ColorUsedService,
    private wowService: NgwWowService
  ) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event) => {
        // Reload WoW animations when done navigating to page,
        // but you are free to call it whenever/wherever you like
        this.wowService.init();
      });
  }

  violet: string = this.colorUsedService.violet;
  pink: string = this.colorUsedService.pink;
  lightPink: string = this.colorUsedService.lightPink;
  orange: string = this.colorUsedService.orange;
  blue: string = this.colorUsedService.blue;
  green: string = this.colorUsedService.green;

  clipBoardsInHome(clipBoards: IClipBoard[]) {
    this.clipBoards = clipBoards;
    console.log(this.clipBoards);
  }

  ngOnInit() {
    // you can subscribe to WOW observable to react when an element is revealed
    this.wowSubscription = this.wowService.itemRevealed$.subscribe(
      (item: HTMLElement) => {
        // do whatever you want with revealed element
      }
    );
  }

  ngOnDestroy() {
    // unsubscribe (if necessary) to WOW observable to prevent memory leaks
    this.wowSubscription.unsubscribe();
  }

  // Add Clipboard to list
  onAddClipBoardList(content: NgForm) {
    let newContent = content.value;
    this.clipBoards.push(newContent);

    /*this.clipBoardService.postClipBoard(content).subscribe((r) => {
      console.log(r);
    });
    */
    content.form.reset();
    console.log(this.clipBoards);
  }

  // Search method
  onClipBoardSerachList(searchInput: NgForm) {
    if (
      searchInput.value.searchInput === undefined ||
      searchInput.value.searchInput === ''
    ) {
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
