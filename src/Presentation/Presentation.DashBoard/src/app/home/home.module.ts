import { NgModule, Renderer2, RendererFactory2 } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './home.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { ClipboardModule } from 'ngx-clipboard';

import { ClipBoardComponent } from '../clipBoard/clipBoard.component';
import { ClipBoardItemComponent } from '../clipBoard/clipBoard-item/clipBoard-item.component';
import { HomeRoutingModule } from './home.routing.module';
import { ColorUsedService } from '../shared/services/color-used.service';
import { SpinnerComponent } from '../shared/spinner/spinner/spinner.component';
import { MobileViewService } from '../shared/services/mobile-view.service';

// define a function that creates a renderer for the module
export function rendererFactory(rendererFactory: RendererFactory2) {
  return rendererFactory.createRenderer(null, null);
}
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    HomeRoutingModule,
    NgbModule,
    InfiniteScrollModule,
    HttpClientModule,
    ClipboardModule,
  ],
  declarations: [
    HomeComponent,
    ClipBoardComponent,
    ClipBoardItemComponent,
    SpinnerComponent,
  ],
  exports: [HomeComponent],
  providers: [
    {
      provide: Renderer2,
      useFactory: rendererFactory,
      deps: [RendererFactory2],
    },

    ColorUsedService,
    MobileViewService,
    Clipboard,
  ],
})
export class HomeModule {}
