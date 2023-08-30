import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { RouterModule } from "@angular/router";
import { AppRoutingModule } from "./app.routing.module";
import { NgwWowModule } from "ngx-wow";

import { AppComponent } from "./app.component";
import { NavbarComponent } from "./shared/navbar/navbar.component";
import { FooterComponent } from "./shared/footer/footer.component";
import { HomeModule } from "./home/home.module";
import { LoginComponent } from "./login/login.component";
import { ColorUsedService } from "./shared/services/color-used.service";
import { AuthService } from "./shared/services/auth.service";
import { AuthGuard } from "./auth/auth.guard";
import { ErrorModalComponent } from "./shared/modals/error-modal/error-modal.component";
import { MatDialogModule } from "@angular/material/dialog";

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    LoginComponent,
    ErrorModalComponent,
  ],
  imports: [
    BrowserModule,
    NgbModule,
    FormsModule,
    RouterModule,
    AppRoutingModule,
    NgwWowModule,
    HomeModule,
    MatDialogModule,
  ],
  providers: [ColorUsedService, AuthService, AuthGuard],
  bootstrap: [AppComponent],
})
export class AppModule {}
