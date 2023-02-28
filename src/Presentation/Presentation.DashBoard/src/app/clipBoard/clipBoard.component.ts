import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BehaviorSubject, map, of } from 'rxjs';
import { tap, take, takeWhile } from 'rxjs/operators';

import { ClipBoardService } from './clipBoard.service';
import { IClipBoard } from './clipBoard.model';
import { ColorUsedService } from '../help/color-used.service';

@Component({
  selector: 'app-clipBoard',
  templateUrl: './clipBoard.component.html',
  styleUrls: ['./clipBoard.component.css'],
})
export class ClipBoardComponent implements OnInit {
  clipBoards: IClipBoard[];

  finished: boolean = true;

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

  dataTest = of([
    {
      content:
        'Angular is running in development mode. Call enableProdMode() to enable production mode.',
    },
    {
      content:
        'A wiki is a web-based collaborative platform that enables users to store, create and modify content in an organized manner. The term comes from the word wiki wiki, which means fast in Hawaiian.',
    },
    {
      content:
        'The Japanese fire-bellied newt (Cynops pyrrhogaster) consists of four distinct varieties, formally recognized together as a single species. Its upper body is dark and its lower regions bright red; coloration varies with age, genetics, and region. Adults are 8 to 15 cm (3 to 6 in) long. They are found on many Japanese islands. Their habitats include bodies of water, forests, and grasslands. They breed from spring to the beginning of summer. Eggs are laid separately, hatching after about three weeks. They grow from larval to juvenile form in five to six months. Juveniles eat soil-dwelling prey; adults eat insects, tadpoles, and the eggs of their own species. They have multiple adaptations to avoid predators, including containing tetrodotoxin, a neurotoxin. Several aspects of their biology have been studied, including their ability to regrow lost body parts. Currently, their population is declining, and they face threats from disease and the pet trade',
    },
    {
      content:
        'Angular is running in development mode. Call enableProdMode() to enable production mode.',
    },
    {
      content:
        'A wiki is a web-based collaborative platform that enables users to store, create and modify content in an organized manner. The term comes from the word wiki wiki, which means fast in Hawaiian.',
    },
    {
      content:
        'The Japanese fire-bellied newt (Cynops pyrrhogaster) consists of four distinct varieties, formally recognized together as a single species. Its upper body is dark and its lower regions bright red; coloration varies with age, genetics, and region. Adults are 8 to 15 cm (3 to 6 in) long. They are found on many Japanese islands. Their habitats include bodies of water, forests, and grasslands. They breed from spring to the beginning of summer. Eggs are laid separately, hatching after about three weeks. They grow from larval to juvenile form in five to six months. Juveniles eat soil-dwelling prey; adults eat insects, tadpoles, and the eggs of their own species. They have multiple adaptations to avoid predators, including containing tetrodotoxin, a neurotoxin. Several aspects of their biology have been studied, including their ability to regrow lost body parts. Currently, their population is declining, and they face threats from disease and the pet trade',
    },
    {
      content:
        'Angular is running in development mode. Call enableProdMode() to enable production mode.',
    },
    {
      content:
        'A wiki is a web-based collaborative platform that enables users to store, create and modify content in an organized manner. The term comes from the word wiki wiki, which means fast in Hawaiian.',
    },
    {
      content:
        'The Japanese fire-bellied newt (Cynops pyrrhogaster) consists of four distinct varieties, formally recognized together as a single species. Its upper body is dark and its lower regions bright red; coloration varies with age, genetics, and region. Adults are 8 to 15 cm (3 to 6 in) long. They are found on many Japanese islands. Their habitats include bodies of water, forests, and grasslands. They breed from spring to the beginning of summer. Eggs are laid separately, hatching after about three weeks. They grow from larval to juvenile form in five to six months. Juveniles eat soil-dwelling prey; adults eat insects, tadpoles, and the eggs of their own species. They have multiple adaptations to avoid predators, including containing tetrodotoxin, a neurotoxin. Several aspects of their biology have been studied, including their ability to regrow lost body parts. Currently, their population is declining, and they face threats from disease and the pet trade',
    },
    {
      content:
        'Angular is running in development mode. Call enableProdMode() to enable production mode.',
    },
    {
      content:
        'A wiki is a web-based collaborative platform that enables users to store, create and modify content in an organized manner. The term comes from the word wiki wiki, which means fast in Hawaiian.',
    },
    {
      content:
        'The Japanese fire-bellied newt (Cynops pyrrhogaster) consists of four distinct varieties, formally recognized together as a single species. Its upper body is dark and its lower regions bright red; coloration varies with age, genetics, and region. Adults are 8 to 15 cm (3 to 6 in) long. They are found on many Japanese islands. Their habitats include bodies of water, forests, and grasslands. They breed from spring to the beginning of summer. Eggs are laid separately, hatching after about three weeks. They grow from larval to juvenile form in five to six months. Juveniles eat soil-dwelling prey; adults eat insects, tadpoles, and the eggs of their own species. They have multiple adaptations to avoid predators, including containing tetrodotoxin, a neurotoxin. Several aspects of their biology have been studied, including their ability to regrow lost body parts. Currently, their population is declining, and they face threats from disease and the pet trade',
    },
  ]);

  ngOnInit(): void {
    /*  this.clipBoardService
      .getClipBoard()
      .pipe(
        map((get) => get.list),
        tap((r) => console.log(r))
      )
      .subscribe(
        (getClipBoardResult) => (this.clipBoards = getClipBoardResult)
      ); */

    this.dataTest
      .pipe(
        map((list) => list),

        tap((r) => console.log(r))
      )
      .subscribe({
        next: (getClipBoardResult) => (this.clipBoards = getClipBoardResult),
      });
  }

  // Add Clipboard to list
  onAddClipBoardList(clipBoardInput: NgForm) {
    const content = clipBoardInput.value.clipBoardInput;
    console.log(content);
    this.clipBoards.push(content);

    /*  this.clipBoardService.postClipBoard(content).subscribe((r) => {
      console.log(r);
    }); */
    clipBoardInput.form.reset();
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

  onScroll() {}
}
