import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Renderer2, RendererFactory2 } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app.routing.module';
import { NgwWowModule } from 'ngx-wow';

import { AppComponent } from './app.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';
import { LoginComponent } from './login/login.component';
import { ColorUsedService } from './shared/services/color-used.service';
import { AuthService } from './shared/services/auth.service';
import { AuthGuard } from './auth/auth.guard';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { HomeComponent } from './home/home.component';
import { ClipBoardComponent } from './clipBoard/clipBoard.component';
import { ClipBoardItemComponent } from './clipBoard/clipBoard-item/clipBoard-item.component';
import { AddOrEditClipboardComponent } from './shared/modals/addOrEditClipboard-modal/addOrEditClipboard-modal.component';
import { SignInModalComponent } from './shared/modals/sign-in-modal/sign-in-modal.component';
import { ForgotPasswordModalComponent } from './shared/modals/forgot-password-modal/forgot-password-modal.component';
import { SignUpModalComponent } from './shared/modals/sign-up-modal/sign-up-modal.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { InterceptorService } from './shared/services/interceptor.service';
import { MobileViewService } from './shared/services/mobile-view.service';
import { ClipBoardService } from './shared/services/clipBoard.service';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { ContactUsComponent } from './shared/contact-us/contact-us.component';
import { SpinnerComponent } from './shared/spinner/spinner.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { environment } from 'src/environments/environment';
import { ServiceWorkerModule } from '@angular/service-worker';
import { ChangePasswordModalComponent } from './shared/modals/change-password-modal/change-password-modal.component';
import { ResetPasswordComponent } from './shared/reset-password/reset-password.component';

// define a function that creates a renderer for the module
export function rendererFactory(rendererFactory: RendererFactory2) {
  return rendererFactory.createRenderer(null, null);
}
@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    LoginComponent,
    HomeComponent,
    ClipBoardComponent,
    ClipBoardItemComponent,
    AddOrEditClipboardComponent,
    SignInModalComponent,
    ForgotPasswordModalComponent,
    SignUpModalComponent,
    ChangePasswordModalComponent,
    ResetPasswordComponent,
    ContactUsComponent,
    SpinnerComponent,
  ],
  imports: [
    BrowserModule,
    NgbModule,
    FormsModule,
    RouterModule,
    AppRoutingModule,
    HttpClientModule,
    NgwWowModule,
    MatDialogModule,
    InfiniteScrollModule,
    MatCardModule,
    MatButtonModule,

    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: environment.production,
    }),
  ],
  providers: [
    ColorUsedService,
    AuthService,
    AuthGuard,
    {
      provide: Renderer2,
      useFactory: rendererFactory,
      deps: [RendererFactory2],
    },

    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true,
    },
    MobileViewService,
    ClipBoardService,
    ClipBoardComponent,
    NavbarComponent,
    {
      provide: MatDialogRef,
      useValue: {},
    },
    {
      provide: MAT_DIALOG_DATA,
      useValue: {},
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
