import { AfterViewInit, Component, OnDestroy, OnInit } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { NgwWowService } from "ngx-wow";
import { Subscription } from "rxjs";
import { debounceTime, filter } from "rxjs/operators";
import { ColorUsedService } from "../shared/services/color-used.service";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { SignInModalComponent } from "../shared/modals/sign-in-modal/sign-in-modal.component";
import { NavbarComponent } from "../shared/navbar/navbar.component";
@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent implements OnInit {
  private wowSubscription: Subscription;

  constructor(
    private router: Router,
    private colorUsedService: ColorUsedService,
    private wowService: NgwWowService,
    public dialog: MatDialog
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
  white: string = this.colorUsedService.white;
  black: string = this.colorUsedService.black;

  ngOnInit() {
    // you can subscribe to WOW observable to react when an element is revealed
    this.wowSubscription = this.wowService.itemRevealed$.subscribe(
      (item: HTMLElement) => {
        // do whatever you want with revealed element
      }
    );

    // handel dark mode after logout ....
    const changeTheme = document.querySelector(
      ".changetheme"
    ) as HTMLElement | null;

    const btnGetStart = document.querySelector(
      "#btnGetStart"
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

      if (btnGetStart) {
        btnGetStart.classList.remove("btn-outline-dark");
        btnGetStart.classList.add("btn-light");
      }

      // in light mode
    } else {
      if (changeTheme) {
        changeTheme.style.color = this.black;
      }

      if (hero) {
        hero.style.backgroundImage = 'url("assets/img/theme/home.jpg")';
      }

      if (btnGetStart) {
        btnGetStart.classList.remove("btn-light");
        btnGetStart.classList.add("btn-outline-dark");
      }
    }
  }

  openSignInDialog() {
    /* const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    //dialogConfig.disableClose = true;
    dialogConfig.hasBackdrop = true; */

    this.dialog.open(SignInModalComponent);
  }

  ngOnDestroy() {
    // unsubscribe (if necessary) to WOW observable to prevent memory leaks
    this.wowSubscription.unsubscribe();
  }
}
