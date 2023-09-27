import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  Inject,
  ViewChild,
  ElementRef,
  AfterViewInit,
} from "@angular/core";
import { NgForm } from "@angular/forms";
import { filter, fromEvent, map, takeWhile, tap, timeout } from "rxjs";

import { ClipBoardService } from "../shared/services/clipBoard.service";
import { IClipBoard } from "./IClipBoard";
import { ColorUsedService } from "../shared/services/color-used.service";
import { MobileViewService } from "../shared/services/mobile-view.service";
import { MatDialog } from "@angular/material/dialog";
import { AddOrEditClipboardComponent } from "../shared/modals/addOrEditClipboard-modal/addOrEditClipboard-modal.component";
import { Router } from "@angular/router";
import { NoopScrollStrategy } from "@angular/cdk/overlay";
import { NavbarComponent } from "../shared/navbar/navbar.component";
import Swal from "sweetalert2";

@Component({
  selector: "app-clipBoard",
  templateUrl: "./clipBoard.component.html",
  styleUrls: ["./clipBoard.component.scss"],
})
export class ClipBoardComponent implements OnInit, AfterViewInit {
  clipBoards: IClipBoard[];

  finished: boolean = true;

  @ViewChild("newButtonClipBoard") myElementRef: ElementRef;

  constructor(
    public clipBoardService: ClipBoardService,
    private colorUsedService: ColorUsedService,
    private mobileViewService: MobileViewService,
    public dialog: MatDialog,
    private navbarComponent: NavbarComponent,
    private router: Router
  ) {}

  violet: string = this.colorUsedService.violet;
  pink: string = this.colorUsedService.pink;
  orange: string = this.colorUsedService.orange;
  blue: string = this.colorUsedService.blue;
  green: string = this.colorUsedService.green;
  white: string = this.colorUsedService.white;
  black: string = this.colorUsedService.black;

  isSearched: boolean = false;
  isLoading: boolean = false;

  totalCount: number | null = null; // Initialize totalCount as null
  skip: number = 0;
  take: number = 0;

  ngOnInit(): void {
    this.onScrollDown();

    // impelement dark Mode if earlier checked darkMode toggler-----------
    const changeTheme = document.querySelector(
      ".changetheme"
    ) as HTMLElement | null;

    const hero = document.querySelector(".hero") as HTMLElement | null;

    //in dark mode
    if (localStorage.getItem("isDarkMode") == "true") {
      if (changeTheme) {
        changeTheme.style.color = this.white;
      }

      if (hero) {
        hero.style.backgroundImage = 'url("assets/img/theme/dark-home.jpg")';
      }

      // in light mode
    } else {
      if (changeTheme) {
        changeTheme.style.color = this.black;
      }

      if (hero) {
        hero.style.backgroundImage = 'url("assets/img/theme/home.jpg")';
      }
    }
  }
  ngAfterViewInit(): void {
    if (window.innerWidth < 500) {
      const newButtonClipBoardElement = this.myElementRef.nativeElement;
      this.mobileViewService.resizeEvent(newButtonClipBoardElement, "d-grid");
    }
    fromEvent(window, "resize").subscribe(() => {
      if (window.innerWidth < 500) {
        const newButtonClipBoardElement = this.myElementRef.nativeElement;
        this.mobileViewService.resizeEvent(newButtonClipBoardElement, "d-grid");
      }
    });
  }

  openAddToClipBoardDialog() {
    const dialogRef = this.dialog.open(AddOrEditClipboardComponent, {
      backdropClass: "true",
      scrollStrategy: new NoopScrollStrategy(),
    });

    /*   dialogRef.updatePosition({
      right: "500px", // Set right position
      top: "600px", // Set top position
    }); */
  }

  // Add Clipboard to list
  onAddClipBoardList(content: NgForm) {
    var newContent = content.value;

    this.clipBoardService.AddToClipBoard(newContent).subscribe({
      // handle successful sign-up response
      next: () => {
        // Reload the page after adding a new clipboard item
        if (localStorage.getItem("isDarkMode") == "true") {
          window.location.reload();
          this.navbarComponent.onChangeDarkMode = true;
          this.navbarComponent.onChangeThemeColor();

          console.log(this.navbarComponent.onChangeDarkMode);
        } else {
          window.location.reload();
        }
      },
      // handle error
      error: (errMes) => {
        console.error(errMes),
          Swal.fire({
            title: "Error!",
            text: errMes.error.detail,
            icon: "error",
            confirmButtonColor: this.violet,
          });
      },
    });
  }

  // Edit Clipboard
  onUpdateClipBoard(editedContent: NgForm, id: number) {
    var editedContentValue = String(Object.values(editedContent.value)[0]);

    this.clipBoardService.UpdateClipBoard(editedContentValue, id).subscribe({
      // handle successful sign-up response
      next: (response) => {
        // console.log(response),
        // Reload the page after adding a new clipboard item

        window.location.reload();
      },
      // handle error
      error: (errMes) => {
        //console.error(errMes),
        Swal.fire({
          title: "Error!",
          text: errMes.error.detail,
          icon: "error",
          confirmButtonColor: this.violet,
        });
      },
    });
  }

  // Search method
  onClipBoardSearchList(searchQuery: NgForm) {
    if (
      searchQuery.value.searchQuery === undefined ||
      searchQuery.value.searchQuery === ""
    ) {
      //console.log(this.clipBoards);
      return this.clipBoards;
    } else {
      var clipBoardFilterd = this.clipBoards.filter((clipBoard: IClipBoard) =>
        clipBoard.content
          .toLowerCase()
          .includes(searchQuery.value.searchQuery.toLowerCase())
      );

      return clipBoardFilterd;
    }
  }

  onScrollDown() {
    if (!this.isLoading) {
      this.isLoading = true;
      if (this.totalCount === null || this.totalCount >= this.take) {
        this.take += 10;
        this.clipBoardService
          .getClipBoard(this.skip, this.take)
          .pipe(
            tap((get) => {
              this.totalCount = get.totalCount;
            })
          )
          .subscribe({
            next: (res) => {
              this.clipBoards = res.list;
              this.isLoading = false; // Move isLoading inside the next callback
            },
          });
      } else {
        this.isLoading = false; // Set isLoading to false if the condition is not met
      }
    }
  }
}
