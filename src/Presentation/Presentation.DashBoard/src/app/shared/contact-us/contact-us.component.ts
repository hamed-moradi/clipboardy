import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ColorUsedService } from '../services/color-used.service';

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
  styleUrls: ['./contact-us.component.scss'],
})
export class ContactUsComponent implements OnInit, AfterViewInit {
  @ViewChild('ContactUs')
  ContactUsElementRef: ElementRef;

  constructor(
    private renderer: Renderer2,
    private colorService: ColorUsedService
  ) {}

  white: string = this.colorService.white;
  black: string = this.colorService.black;

  darkModeToggle = document.querySelector(
    '#darkModeToggle'
  ) as HTMLElement | null;

  ngOnInit(): void {
    const hero = document.querySelector('.hero') as HTMLElement | null;

    const contactUs = document.querySelector(
      '.container'
    ) as HTMLElement | null;

    if (window.innerWidth > 600) {
      if (contactUs) contactUs.classList.add('d-flex');
    }

    // handel dark mode
    const changeTheme = document.querySelector(
      '.changetheme'
    ) as HTMLElement | null;

    const ImgContactUS = document.querySelector(
      '#ImgContactUS'
    ) as HTMLElement | null;

    const signInDark = document.querySelector(
      '.signInDark'
    ) as HTMLElement | null;

    const signOutDark = document.querySelector(
      '.signOutDark'
    ) as HTMLElement | null;

    //in dark mode
    if (this.darkModeToggle?.getAttribute('ng-reflect-model') == 'true') {
      if (hero) {
        hero.style.backgroundImage = 'url("assets/img/theme/dark-home.jpg")';
      }
      if (changeTheme) {
        changeTheme.style.color = this.white;
      }

      if (ImgContactUS) {
        ImgContactUS.setAttribute('src', 'assets/img/theme/contactUS-dark.png');
      }

      if (signInDark) signInDark.style.removeProperty('color');

      if (signOutDark) signOutDark.style.color = this.white;
      // in light mode
    } else {
      if (hero) {
        hero.style.removeProperty('background-image');
        hero.style.removeProperty('background');
      }

      if (changeTheme) {
        changeTheme.style.color = this.black;
      }

      if (ImgContactUS)
        ImgContactUS.setAttribute('src', 'assets/img/theme/contactUS.png');
    }
  }

  ngAfterViewInit(): void {
    const ContactUsElement = this.ContactUsElementRef.nativeElement;

    this.renderer.addClass(ContactUsElement, 'justify-content-center');
    this.renderer.listen(window, 'resize', (event) => {
      const newWidth = event.target.innerWidth;
      if (newWidth > 600) {
        this.renderer.addClass(ContactUsElement, 'justify-content-center');
      } else {
        this.renderer.removeClass(ContactUsElement, 'justify-content-center');
      }

      this.renderer.addClass(ContactUsElement, 'd-flex');
      this.renderer.listen(window, 'resize', (event) => {
        const newWidth = event.target.innerWidth;
        if (newWidth > 600) {
          this.renderer.addClass(ContactUsElement, 'd-flex');
        } else {
          this.renderer.removeClass(ContactUsElement, 'd-flex');
        }
      });
    });
  }
}
