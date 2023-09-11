import { Component, OnInit, Renderer2, ElementRef } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Location } from '@angular/common';
import { filter, Subscription } from 'rxjs';

import { ColorUsedService } from '../../shared/services/color-used.service';
import { AuthService } from '../services/auth.service';
import { __values } from 'tslib';
import { MatDialog } from '@angular/material/dialog';
import { SignInModalComponent } from '../modals/sign-in-modal/sign-in-modal.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
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

  navbarMobility: boolean = false;
  isDarkModeEnabled: boolean = false;

  ngOnInit() {
    // Initialize dark mode based on the initial state
    console.log(this.isDarkModeEnabled + 'ngOnInit');
    if (this.isDarkModeEnabled) {
      this.enableDarkMode();
    }

    var navbar: HTMLElement =
      this.element.nativeElement.children[0].children[0];

    //add p-4 bootstrap calss into container-fluid
    navbar.classList.add('p-4');

    //add headroomNoTop calss into container-fluid
    this._router = this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event) => {
        this.renderer.listen('window', 'scroll', (event) => {
          const number = window.scrollY;

          const headroomNoTop = document.querySelector(
            '.headroomNoTop'
          ) as HTMLElement | null;

          const containerFluid = document.querySelector(
            '#containerFluid'
          ) as HTMLElement | null;

          const homeButton = document.querySelector(
            '#homeButton'
          ) as HTMLElement | null;

          if (number > 150 || window.scrollY > 150) {
            // not In top of page
            navbar.classList.add('headroomNoTop');
            navbar.classList.remove('p-4');

            if (headroomNoTop) {
              if (this.isDarkModeEnabled) {
                headroomNoTop.style.backgroundColor = this.orange;
              } else {
                headroomNoTop.style.backgroundColor = this.white;
              }
            }

            if (homeButton) {
              if (this.isDarkModeEnabled) {
                homeButton.style.color = this.white;
              } else {
                homeButton.style.removeProperty('color');
              }
            }
          } else {
            // top of page

            navbar.classList.remove('headroomNoTop');
            navbar.classList.add('p-4');

            //Remove backGroundColor from navBar
            if (containerFluid)
              containerFluid.style.removeProperty('background-color');

            /*
            if (headroomNoTop) {
              if (this.isDarkModeEnabled) {
                headroomNoTop.style.backgroundColor = this.orange;
              } else {
                headroomNoTop.style.backgroundColor = this.white;
              } */

            if (homeButton) {
              if (this.isDarkModeEnabled) {
                homeButton.style.color = this.white;
              } else {
                homeButton.style.removeProperty('color');
              }
            }
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
    const navbarButtonCollopse = document.getElementById('navButton');
    const navbarCollopse = document.getElementById('navbarNav');
    navbarButtonCollopse?.setAttribute('aria-expanded', 'false');
    navbarButtonCollopse?.setAttribute('class', 'navbar-toggler collapsed');

    navbarCollopse?.setAttribute(
      'class',
      'navbar-collapse col-2 justify-content-end collapse'
    );
  }

  onChangeThemeColor() {
    console.log(this.isDarkModeEnabled + 'onChangeTheme');
    const body = document.getElementsByTagName('body')[0];
    if (this.isDarkModeEnabled === false) {
      body.classList.remove('dark-theme');
      const hero = document.querySelector('.hero') as HTMLElement | null;
      if (hero) {
        hero.style.backgroundImage = 'url("assets/img/theme/home.jpg")';
      }
    } else {
      body.classList.add('dark-theme');
      const hero = document.querySelector('.hero') as HTMLElement | null;
      if (hero) {
        hero.style.backgroundImage = 'url("assets/img/theme/dark-home.jpg")';
      }
    }
  }

  private enableDarkMode() {
    const body = document.getElementsByTagName('body')[0];
    body.classList.add('dark-theme');

    const hero = document.querySelector('.hero') as HTMLElement | null;
    if (hero) {
      hero.style.backgroundImage = 'url("assets/img/theme/dark-home.jpg")';
    }
  }
}
