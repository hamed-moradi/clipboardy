import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  constructor() {}

  lengthInfoMessage: string = 'The minimum lenght of characters must be 3';
  fillAllFieldsMessage: string = 'Please fill all fields or valid Email';
}
