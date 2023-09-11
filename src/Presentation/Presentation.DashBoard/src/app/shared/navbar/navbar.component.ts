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

          if (number > 150 || window.scrollY > 150) {
            // not In top of page
            navbar.classList.add('headroomNoTop');
            navbar.classList.remove('p-4');

            if (headroomNoTop) {
              if (this.isDarkModeEnabled) {
                headroomNoTop.style.backgroundColor = this.violet;
              } else {
                headroomNoTop.style.backgroundColor = this.white;
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

            /*  if (this.homeButton) {
              if (this.isDarkModeEnabled) {
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
    const hero = document.querySelector('.hero') as HTMLElement | null;
    const body = document.getElementsByTagName('body')[0];

    const headroomNoTop = document.querySelector(
      '.headroomNoTop'
    ) as HTMLElement | null;

    const logoImg = document.querySelector('#logoImg') as HTMLElement | null;

    const logoBrand = document.querySelector(
      '#logoBrand'
    ) as HTMLElement | null;

    const homeButton = document.querySelector(
      '#homeButton'
    ) as HTMLElement | null;

    const contactUsButton = document.querySelector(
      '#contactUsButton'
    ) as HTMLElement | null;

    const signInButton = document.querySelector(
      '#signInButton'
    ) as HTMLElement | null;

    const signOutButton = document.querySelector(
      '#signOutButton'
    ) as HTMLElement | null;

    const darkIcon = document.querySelector('#darkIcon') as HTMLElement | null;

    //dark Mode is disabled
    if (this.isDarkModeEnabled === false) {
      body.classList.remove('dark-theme');

      if (hero) {
        hero.style.backgroundImage = 'url("assets/img/theme/home.jpg")';
      }

      if (headroomNoTop) {
        headroomNoTop.style.backgroundColor = this.white;
      }

      if (logoImg) {
        logoImg.style.removeProperty('background-image');
      }

      if (logoBrand) {
        logoBrand.style.removeProperty('color');
      }

      if (homeButton) {
        homeButton.style.removeProperty('color');
      }

      if (contactUsButton) {
        contactUsButton.style.removeProperty('color');
      }

      if (signInButton) {
        signInButton.style.removeProperty('color');
      }

      if (signOutButton) {
        signOutButton.style.removeProperty('color');
      }

      if (darkIcon) {
        darkIcon.style.removeProperty('background-color');
        darkIcon.style.removeProperty('border-radius');
      }

      //dark Mode Enabled
    } else {
      body.classList.add('dark-theme');

      console.log(hero);
      if (hero) {
        hero.style.backgroundImage = 'url("assets/img/theme/dark-home.jpg")';
      }

      if (headroomNoTop) {
        headroomNoTop.style.backgroundColor = this.violet;
      }

      if (logoImg) {
        logoImg.style.backgroundImage =
          'url("assets/img/Logo/clipBoardyWhite.png")';
      }

      if (logoBrand) {
        logoBrand.style.color = this.white;
      }

      if (homeButton) {
        homeButton.style.color = this.white;
      }

      if (contactUsButton) {
        contactUsButton.style.color = this.white;
      }

      if (signInButton) {
        signInButton.style.color = this.white;
      }

      if (signOutButton) {
        signOutButton.style.color = this.white;
      }

      if (darkIcon) {
        darkIcon.style.backgroundColor = this.white;
        darkIcon.style.borderRadius = '12px';
      }
    }
  }

  private enableDarkMode() {
    const hero = document.querySelector('.hero') as HTMLElement | null;
    const body = document.getElementsByTagName('body')[0];

    const logoImg = document.querySelector('#logoImg') as HTMLElement | null;

    const logoBrand = document.querySelector(
      '#logoBrand'
    ) as HTMLElement | null;

    const homeButton = document.querySelector(
      '#homeButton'
    ) as HTMLElement | null;

    const contactUsButton = document.querySelector(
      '#contactUsButton'
    ) as HTMLElement | null;

    const signInButton = document.querySelector(
      '#signInButton'
    ) as HTMLElement | null;

    const signOutButton = document.querySelector(
      '#signOutButton'
    ) as HTMLElement | null;

    const darkIcon = document.querySelector('#darkIcon') as HTMLElement | null;

    body.classList.add('dark-theme');

    if (hero) {
      hero.style.backgroundImage = 'url("assets/img/theme/dark-home.jpg")';
    }

    if (logoImg) {
      logoImg.style.backgroundImage =
        'url("assets/img/Logo/clipBoardyWhite.png")';
    }

    if (logoBrand) {
      logoBrand.style.color = this.white;
    }

    if (homeButton) {
      homeButton.style.color = this.white;
    }

    if (contactUsButton) {
      contactUsButton.style.color = this.white;
    }

    if (signInButton) {
      signInButton.style.color = this.white;
    }

    if (signOutButton) {
      signOutButton.style.color = this.white;
    }

    if (darkIcon) {
      darkIcon.style.backgroundColor = this.white;
      darkIcon.style.borderRadius = '12px';
    }
  }
}
