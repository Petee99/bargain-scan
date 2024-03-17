import { HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, tap, switchMap, filter, take, finalize } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class WebReqInterceptorService implements HttpInterceptor{
  private refreshTokenInProgress = false;
  private refreshTokenSubject = new BehaviorSubject(null);
  constructor(private authService: AuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error.status === 401 && error){
          if(this.refreshTokenInProgress){
            return this.refreshTokenSubject.pipe(
              take(1),
              switchMap(() => next.handle(request))
            );
          }
          else{
            this.refreshTokenInProgress = true;
            this.refreshTokenSubject.next(null);
            return this.refreshAccessToken().pipe(
              switchMap(() => {
              this.refreshTokenSubject.next("next");
              return next.handle(request);
              }),
              finalize(() => (this.refreshTokenInProgress = false))
            );
          }
        }
        else{
          return throwError((error));
        }
      })
    )
  }

  refreshAccessToken() {
    return this.authService.refreshAccessToken().pipe(
      tap(() => {
      })
    )
  }
}
