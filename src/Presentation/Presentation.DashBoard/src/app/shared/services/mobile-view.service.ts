import { Injectable, Renderer2 } from '@angular/core';

@Injectable()
export class MobileViewService {
  constructor(private renderer: Renderer2) {}

  resizeEvent(idElement: HTMLElement, classElement: string) {
    this.renderer.addClass(idElement, classElement);
    this.renderer.listen(window, 'resize', (event) => {
      const newWidth = event.target.innerWidth;
      if (newWidth < 500) {
        this.renderer.addClass(idElement, classElement);
      } else {
        this.renderer.removeClass(idElement, classElement);
      }
    });
  }
}
