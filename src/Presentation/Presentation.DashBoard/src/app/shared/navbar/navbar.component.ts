import { Component, OnInit, Renderer2, ElementRef } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Location } from '@angular/common';
import { filter, Subscription } from 'rxjs';

import { ColorUsedService } from 'src/app/help/color-used.service';
import { AuthService } from '../../auth/auth.service';

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
    private authService: AuthService
  ) {}

  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  navbarMobility: boolean = false;

  ngOnInit() {
    var navbar: HTMLElement =
      this.element.nativeElement.children[0].children[0];

    //add p-4 bootstrap calss into container-fluid
    navbar.classList.add('p-4');

    //add headroom--not-top calss into container-fluid
    this._router = this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event) => {
        this.renderer.listen('window', 'scroll', (event) => {
          const number = window.scrollY;
          if (number > 150 || window.pageYOffset > 150) {
            // add logic
            navbar.classList.add('headroom--not-top');
            navbar.classList.remove('p-4');
          } else {
            // remove logic
            navbar.classList.remove('headroom--not-top');
            navbar.classList.add('p-4');
          }
        });
      });
  }

  isHome() {
    var titlee = this.location.prepareExternalUrl(this.location.path());

    if (titlee === '#/home') {
      return true;
    } else {
      return false;
    }
  }
  isDocumentation() {
    var titlee = this.location.prepareExternalUrl(this.location.path());
    if (titlee === '#/documentation') {
      return true;
    } else {
      return false;
    }
  }
}
