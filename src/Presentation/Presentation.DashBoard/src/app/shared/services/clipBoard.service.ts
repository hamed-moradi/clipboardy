import { Injectable } from "@angular/core";

import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { map, catchError } from "rxjs/operators";

@Injectable({
  providedIn: "root",
})
export class ClipBoardService {
  baseURL: string = "http://localhost:2020";

  constructor(private http: HttpClient) {}

  getClipBoard(): Observable<any> {
    return this.http.get(this.baseURL + "/api/clipboard/get");
  }

  AddToClipBoard(content: string): Observable<any> {
    return this.http.post(this.baseURL + "/api/clipboard/add", content);
  }

  UpdateClipBoard(content: string, id: number): Observable<any> {
    const body = { content, id };
    return this.http.put(this.baseURL + "/api/clipboard/update", body);
  }
}
