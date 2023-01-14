import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { ColorUsedService } from "../help/color-used.service";


@Component({
  selector:'app-header',
  templateUrl:'./header.component.html',
  styleUrls:['./header.component.css']
})
export class HeaderCoponent implements OnInit {

headerColor:string="";
@Output() featureSelected=new EventEmitter<string>();

constructor(private _headerColor:ColorUsedService){}

ngOnInit(): void {

  this.headerColor=this._headerColor.headerColor;
}
onSelect(feature:string){
  this.featureSelected.emit(feature);
}
}

