import { Component, OnInit } from '@angular/core';
import { ColorUsedService } from '../help/color-used.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  model = {
    left: true,
    middle: false,
    right: false,
  };

  focus: boolean;
  focus1: boolean;
  constructor(private colorUsedService: ColorUsedService) {}
  violet: string = this.colorUsedService.violet;
  pink: string = this.colorUsedService.pink;
  lightPink: string = this.colorUsedService.lightPink;
  orange: string = this.colorUsedService.orange;
  blue: string = this.colorUsedService.blue;
  green: string = this.colorUsedService.green;
  ngOnInit() {}
}
