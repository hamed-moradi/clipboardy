export class ClipBoard {
  public typeId?: number;
  public content: string;

  constructor(content: string, typeId?: number) {
    this.content = content;
    this.typeId = typeId;
  }
}
/*
export interface IClipboard {
  typeId?: number;
  content: string;
}
*/
