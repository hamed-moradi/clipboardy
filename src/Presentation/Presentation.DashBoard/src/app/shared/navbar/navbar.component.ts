import { Component, OnInit, Renderer2, ElementRef } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { Location } from "@angular/common";
import { filter, Subscription } from "rxjs";

import { ColorUsedService } from "../../shared/services/color-used.service";
import { AuthService } from "../services/auth.service";
import { __values } from "tslib";
import { MatDialog } from "@angular/material/dialog";
import { SignInModalComponent } from "../modals/sign-in-modal/sign-in-modal.component";

@Component({
  selector: "app-navbar",
  templateUrl: "./navbar.component.html",
  styleUrls: ["./navbar.component.scss"],
})
export class NavbarComponent implements OnInit {
  public isCollapsed = true;
  private lastPoppedUrl?: string;
  private yScrollStack: number[] = [];
  private _router: Subscription;

  constructor(
    public location: Location,
    private router: Router,
    private renderer: Renderer2,
    private element: ElementRef,
    private colorUsed: ColorUsedService,
    public authService: AuthService,
    public dialog: MatDialog
  ) {}

  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;
  white: string = this.colorUsed.white;
  black: string = this.colorUsed.black;

  navbarMobility: boolean = false;
  onChangeDarkMode: boolean;

  ngOnInit() {
    if (localStorage.getItem("isDarkMode") == "true") this.onChangeThemeColor();
    //localStorage.setItem("isDarkMode", String(this.onChangeDarkMode));

    // Initialize dark mode based on the initial state
    console.log(this.onChangeDarkMode + "ngOnInit");
    if (this.onChangeDarkMode) {
      this.enableDarkMode();

      /*      console.log(this.onChangeDarkMode + "ssssssss");
      localStorage.setItem("isDarkMode", String(this.onChangeDarkMode)); */
    }

    var navbar: HTMLElement =
      this.element.nativeElement.children[0].children[0];

    //add p-4 bootstrap calss into container-fluid
    navbar.classList.add("p-4");

    //add headroomNoTop calss into container-fluid
    this._router = this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event) => {
        this.renderer.listen("window", "scroll", (event) => {
          const number = window.scrollY;

          const headroomNoTop = document.querySelector(
            ".headroomNoTop"
          ) as HTMLElement | null;

          const containerFluid = document.querySelector(
            "#containerFluid"
          ) as HTMLElement | null;

          if (number > 150 || window.scrollY > 150) {
            // not In top of page
            navbar.classList.add("headroomNoTop");
            navbar.classList.remove("p-4");

            if (headroomNoTop) {
              if (this.onChangeDarkMode) {
                headroomNoTop.style.backgroundColor = this.violet;
              } else {
                headroomNoTop.style.backgroundColor = this.white;
              }
            }
          } else {
            // top of page

            navbar.classList.remove("headroomNoTop");
            navbar.classList.add("p-4");

            //Remove backGroundColor from navBar
            if (containerFluid)
              containerFluid.style.removeProperty("background-color");

            /*
            if (headroomNoTop) {
              if (this.onChangeDarkMode) {
                headroomNoTop.style.backgroundColor = this.orange;
              } else {
                headroomNoTop.style.backgroundColor = this.white;
              } */

            /*  if (this.homeButton) {
              if (this.onChangeDarkMode) {
                this.homeButton.style.color = this.white;
              } else {
                this.homeButton.style.removeProperty('color');
              }
            } */
          }
        });
      });
  }

  openSignInDialog() {
    this.dialog.open(SignInModalComponent);
  }

  onSignOut() {
    this.authService.logout();
  }
  onCollopseNavbar() {
    const navbarButtonCollopse = document.getElementById("navButton");
    const navbarCollopse = document.getElementById("navbarNav");
    navbarButtonCollopse?.setAttribute("aria-expanded", "false");
    navbarButtonCollopse?.setAttribute("class", "navbar-toggler collapsed");

    navbarCollopse?.setAttribute(
      "class",
      "navbar-collapse col-2 justify-content-end collapse"
    );
  }

  onChangeThemeColor() {
    console.log(this.onChangeDarkMode);
    localStorage.setItem("isDarkMode", String(this.onChangeDarkMode));

    const hero = document.querySelector(".hero") as HTMLElement | null;
    const body = document.getElementsByTagName("body")[0];

    const changeTheme = document.querySelector(
      ".changetheme"
    ) as HTMLElement | null;

    const btnGetStart = document.querySelector(
      "#btnGetStart"
    ) as HTMLElement | null;

    const headroomNoTop = document.querySelector(
      ".headroomNoTop"
    ) as HTMLElement | null;

    /*     const logoImg = document.querySelector('#logoImg') as HTMLElement | null;
     */
    const logoBrand = document.querySelector(
      "#logoBrand"
    ) as HTMLElement | null;

    const darkIcon = document.querySelector("#darkIcon") as HTMLElement | null;

    //dark Mode is disabled
    if (this.onChangeDarkMode === false) {
      body.classList.remove("dark-theme");

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

      if (headroomNoTop) {
        headroomNoTop.style.backgroundColor = this.white;
      }

      /*    if (logoImg) {
        logoImg.style.backgroundImage = 'url("assets/img/logo/clipBoardy.png")';
      } */

      if (logoBrand) {
        logoBrand.style.removeProperty("color");
      }

      if (darkIcon) {
        darkIcon.style.removeProperty("background-color");
        darkIcon.style.removeProperty("border-radius");
      }

      //dark Mode Enabled
    } else {
      body.classList.add("dark-theme");

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

      if (headroomNoTop) {
        headroomNoTop.style.backgroundColor = this.violet;
      }

      /*  if (logoImg) {
        logoImg.style.backgroundImage =
          'url("assets/img/logo/clipBoardyWhite.png")';
      } */

      if (logoBrand) {
        logoBrand.style.color = this.white;
      }

      if (darkIcon) {
        darkIcon.style.backgroundColor = this.white;
        darkIcon.style.borderRadius = "12px";
      }
    }
  }

  private enableDarkMode() {
    const hero = document.querySelector(".hero") as HTMLElement | null;
    const body = document.getElementsByTagName("body")[0];

    /*     const logoImg = document.querySelector('#logoImg') as HTMLElement | null;
     */
    const changeTheme = document.querySelector(
      ".changetheme"
    ) as HTMLElement | null;

    const btnGetStart = document.querySelector(
      "#btnGetStart"
    ) as HTMLElement | null;

    const logoBrand = document.querySelector(
      "#logoBrand"
    ) as HTMLElement | null;

    const darkIcon = document.querySelector("#darkIcon") as HTMLElement | null;

    body.classList.add("dark-theme");

    if (changeTheme) {
      changeTheme.style.color = "#EAE6F0";
    }

    if (hero) {
      hero.style.backgroundImage = 'url("assets/img/theme/dark-home.jpg")';
    }

    if (btnGetStart) {
      btnGetStart.classList.remove("btn-outline-dark");
      btnGetStart.classList.add("btn-light");
    }

    /*    if (logoImg) {
      logoImg.style.backgroundImage =
        'url("assets/img/logo/clipBoardyWhite.png")';
    } */

    if (logoBrand) {
      logoBrand.style.color = this.white;
    }

    if (darkIcon) {
      darkIcon.style.backgroundColor = this.white;
      darkIcon.style.borderRadius = "12px";
    }
  }

  //dynamic logo
  getLogoSrc(): string {
    return this.onChangeDarkMode
      ? "/assets/img/logo/clipBoardyWhite.png"
      : "/assets/img/logo/clipBoardy.png";
  }
}
