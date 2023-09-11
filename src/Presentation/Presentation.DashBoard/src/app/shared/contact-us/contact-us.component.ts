import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
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
export class ContactUsComponent implements OnInit, AfterViewInit {
  @ViewChild("ContactUs")
  ContactUsElementRef: ElementRef;

  constructor(private renderer: Renderer2) {}

  ngOnInit(): void {
    const contactUs = document.querySelector(
      ".container"
    ) as HTMLElement | null;

    if (window.innerWidth > 600) {
      if (contactUs) contactUs.classList.add("d-flex");
    }
  }

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
