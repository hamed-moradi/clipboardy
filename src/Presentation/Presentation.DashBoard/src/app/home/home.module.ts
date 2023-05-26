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
import { AddToClipboardModalComponent } from '../shared/modals/add-to-clipboard-modal/add-to-clipboard-modal.component';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';

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
    MatDialogModule,
  ],
  declarations: [
    HomeComponent,
    ClipBoardComponent,
    ClipBoardItemComponent,
    SpinnerComponent,
    AddToClipboardModalComponent,
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
    ClipBoardComponent,
    Clipboard,
    {
      provide: MatDialogRef,
      useValue: {},
    },
    {
      provide: MAT_DIALOG_DATA,
      useValue: {},
    },
  ],
})
export class HomeModule {}
