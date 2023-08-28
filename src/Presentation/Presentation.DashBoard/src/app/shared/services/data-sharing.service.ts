import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DataSharingService {
  constructor() {}
  private isEditMode: boolean = false;

  setIsEditMode(value: boolean) {
    this.isEditMode = value;
  }

  getIsEditMode() {
    return this.isEditMode;
  }
}
