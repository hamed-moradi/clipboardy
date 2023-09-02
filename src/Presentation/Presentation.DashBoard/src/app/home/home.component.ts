import { ConstantPool } from '@angular/compiler';
import {
  Component,
  ElementRef,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, NavigationEnd } from '@angular/router';
import { NgwWowService } from 'ngx-wow';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { IClipBoard } from '../clipBoard/IClipBoard';
import { ColorUsedService } from '../shared/services/color-used.service';
import { ClipBoardComponent } from '../clipBoard/clipBoard.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit, OnDestroy {
  clipBoards: IClipBoard[];

  @ViewChild(ClipBoardComponent) clipBoardComponent: ClipBoardComponent;

  private wowSubscription: Subscription;

  constructor(
    private router: Router,
    private renderer: Renderer2,
    private elementRef: ElementRef,
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

  ngOnInit() {
    // you can subscribe to WOW observable to react when an element is revealed
    this.wowSubscription = this.wowService.itemRevealed$.subscribe(
      (item: HTMLElement) => {
        // do whatever you want with revealed element
      }
    );

    this.renderer.listen('window', 'scroll', () => {
      const scrollTop =
        document.documentElement.scrollTop ||
        document.body.scrollTop ||
        window.scrollY ||
        0;
      const scrollTopButton =
        this.elementRef.nativeElement.querySelector('#scrollTopButton');

      if (scrollTop >= 500) {
        scrollTopButton.style.display = 'block';
        this.renderer.addClass(scrollTopButton, 'scrollTop');
      } else {
        scrollTopButton.style.display = 'none';
        this.renderer.removeClass(scrollTopButton, 'scrollTop');
      }
    });
  }

  scrollToTop(event: Event) {
    event.preventDefault();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  ngOnDestroy() {
    // unsubscribe (if necessary) to WOW observable to prevent memory leaks
    this.wowSubscription.unsubscribe();
  }
}
