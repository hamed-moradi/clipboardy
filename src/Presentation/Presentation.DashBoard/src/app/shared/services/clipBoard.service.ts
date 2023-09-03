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

  getClipBoard(skip: number, take: number): Observable<any> {
    let httpParams = new HttpParams()
      .set('skip', skip.toString())
      .set('take', take.toString());

    return this.http.get(this.baseURL + '/api/clipboard/get', {
      params: httpParams,
    });
  }

  AddToClipBoard(content: string): Observable<any> {
    return this.http.post(this.baseURL + '/api/clipboard/add', content);
  }

  UpdateClipBoard(content: string, id: number): Observable<any> {
    const body = { content, id };
    return this.http.put(this.baseURL + '/api/clipboard/update', body);
  }

  DeleteClipBoard(id: number | undefined): Observable<any> {
    // Check if the id is defined and is a number
    if (id !== undefined && typeof id === 'number') {
      // Use the params option to pass the id as a query parameter
      let httpParams = new HttpParams().set('id', id.toString());
      return this.http.delete(this.baseURL + '/api/clipboard/delete', {
        params: httpParams,
      });
    } else {
      // Handle the case when the id is undefined or not a number
      const err = new Error('test');
      return throwError(() => err);
    }
  }
}
