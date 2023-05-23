import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, delay, finalize } from 'rxjs';
import { BusyService } from '../service/busy.service';

@Injectable()                                                         //130
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (!request.url.includes('emailExists')){    //check what the url is before engage busyService 203
      this.busyService.busy();    
    }
    return next.handle(request).pipe(
      delay(1000),                                           //loading delay time sec
      finalize(() => this.busyService.idle())      
    );
  }
}
