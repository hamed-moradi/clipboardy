import { Injectable } from '@angular/core';

import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ClipBoardService {
  baseURL: string = 'http://localhost:2020';

  constructor(private http: HttpClient) {}

  getClipBoard(): Observable<any> {
    return this.http.get(this.baseURL + '/home', {
      headers: new HttpHeaders({ customClipBoard: 'Hello!' }),
    });
  }

  postClipBoard(text: string): Observable<any> {
    return this.http.post(this.baseURL + '/home', text);
  }
}
