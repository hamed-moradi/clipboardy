import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { Location, PopStateEvent } from '@angular/common';
import { NgForm } from '@angular/forms';

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

  constructor(
    public location: Location,
    private router: Router,
    private colorUsed: ColorUsedService,
    private authService: AuthService
  ) {}

  pink: string = this.colorUsed.pink;
  lightPink: string = this.colorUsed.lightPink;
  orange: string = this.colorUsed.orange;
  green: string = this.colorUsed.green;
  violet: string = this.colorUsed.violet;
  blue: string = this.colorUsed.blue;

  ngOnInit() {
    this.router.events.subscribe((event) => {
      this.isCollapsed = true;
      if (event instanceof NavigationStart) {
        if (event.url != this.lastPoppedUrl)
          this.yScrollStack.push(window.scrollY);
      } else if (event instanceof NavigationEnd) {
        if (event.url == this.lastPoppedUrl) {
          this.lastPoppedUrl = undefined;
        } else window.scrollTo(0, 0);
      }
    });
    this.location.subscribe((ev: PopStateEvent) => {
      this.lastPoppedUrl = ev.url;
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

  onSignInForm(SignInuserForm: NgForm) {
    if (SignInuserForm.valid) {
      this.authService.login(SignInuserForm.value);
    }
  }

  onSignUpForm(SignUpuserForm: NgForm) {
    if (SignUpuserForm.valid) {
      console.log(SignUpuserForm);
      console.log(SignUpuserForm.value);
    }
  }

  onForgotPassword(ForgotPasswordForm: NgForm) {
    if (ForgotPasswordForm.valid) {
      console.log(ForgotPasswordForm);
      console.log(ForgotPasswordForm.value);
    }
  }
}
