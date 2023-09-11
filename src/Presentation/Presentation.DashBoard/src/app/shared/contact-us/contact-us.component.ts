import {
  AfterViewInit,
  Component,
  ElementRef,
  Renderer2,
  ViewChild,
} from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";

@Component({
  selector: "app-contact-us",
  templateUrl: "./contact-us.component.html",
  styleUrls: ["./contact-us.component.scss"],
})
export class ContactUsComponent implements AfterViewInit {
  @ViewChild("ContactUs")
  ContactUsElementRef: ElementRef;

  constructor(private renderer: Renderer2) {}
  ngAfterViewInit(): void {
    const ContactUsElement = this.ContactUsElementRef.nativeElement;

    this.renderer.addClass(ContactUsElement, "justify-content-center");
    this.renderer.listen(window, "resize", (event) => {
      const newWidth = event.target.innerWidth;
      if (newWidth > 600) {
        this.renderer.addClass(ContactUsElement, "justify-content-center");
      } else {
        this.renderer.removeClass(ContactUsElement, "justify-content-center");
      }

      this.renderer.addClass(ContactUsElement, "d-flex");
      this.renderer.listen(window, "resize", (event) => {
        const newWidth = event.target.innerWidth;
        if (newWidth > 600) {
          this.renderer.addClass(ContactUsElement, "d-flex");
        } else {
          this.renderer.removeClass(ContactUsElement, "d-flex");
        }
      });
    });
  }
}
