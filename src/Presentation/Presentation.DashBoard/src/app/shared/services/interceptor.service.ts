import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpHeaders,
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class InterceptorService implements HttpInterceptor {
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const modifiedRequest = request.clone({
      headers: this.createHeaders(),
    });
    return next.handle(modifiedRequest);
  }

  private createHeaders(): HttpHeaders {
    const headers = new HttpHeaders({
      deviceKey: this.getDeviceKey(),
      deviceName: navigator.userAgent,
      deviceType: this.getDeviceType(),
      authorization: localStorage.getItem('token')!,
    });
    return headers;
  }

  private getDeviceKey(): string {
    const userAgent = navigator.userAgent;
    const hash = this.hashString(userAgent);
    return hash;
  }

  private hashString(str: string): string {
    let hash = 0;
    if (str.length === 0) {
      return hash.toString();
    }
    for (let i = 0; i < str.length; i++) {
      const char = str.charCodeAt(i);
      hash = (hash << 5) - hash + char;
      hash = hash & hash; // Convert to 32-bit integer
    }
    return hash.toString();
  }

  private getDeviceType(): string {
    const userAgent = navigator.userAgent;
    if (/iPad|iPhone|iPod/.test(userAgent)) {
      return 'iOS';
    } else if (/Android/.test(userAgent)) {
      return 'Android';
    } else {
      return 'PC';
    }
  }
}
