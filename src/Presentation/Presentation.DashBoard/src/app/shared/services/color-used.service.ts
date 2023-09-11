import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ColorUsedService {
  constructor() {}
  public violet: string = '#930086';
  public pink: string = '#ff4db2';
  public lightPink: string = '#ffb7ff';
  public blue: string = '#0077ff';
  public orange: string = '#ffad33';
  public green: string = '#1ec8a3';
  public gray: string = '#808080';
  public white: string = '#EAE6F0';
}
