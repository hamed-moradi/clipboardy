import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';

import { HomeComponent } from './home.component';

import { SectionsModule } from '../sections/sections.module';
import { ClipBoardComponent } from '../clipBoard/clipBoard.component';
import { ClipBoardListComponent } from '../clipBoard/clipBoard-list/clipBoard-list.component';
import { ClipBoardItemComponent } from '../clipBoard/clipBoard-list/clipBoard-item/clipBoard-item.component';
import { HomeRoutingModule } from './home.routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    HomeRoutingModule,
    SectionsModule,
    NgbModule,
  ],
  declarations: [
    HomeComponent,
    ClipBoardComponent,
    ClipBoardListComponent,
    ClipBoardItemComponent,
  ],
  exports: [HomeComponent],
  providers: [],
})
export class HomeModule {}
