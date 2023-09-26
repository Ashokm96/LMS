import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { ToastService } from './toast.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private authservice: AuthService,
    private router: Router,
    private toastr: ToastService
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('token');

    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(request).pipe(
      catchError((err) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 400) {
            this.toastr.showError(err.error.message);
          }
          if (err.status === 403) {
            this.toastr.showError("You are not authorize to access this page.")
          }
          if (err.status === 401) {
            if (err.error.message !== "Username/Password incorrect.") {
              this.toastr.showError("Session expired");
            }
            this.authservice.logout();
            this.router.navigate(['/login']);
          }
          if (err.status === 500) {
            this.toastr.showError("Internal Server Error.");
          }
          if (err.status === 0) {
            this.toastr.showError("Server is down, Try again later !");
          }
        }
        return throwError(err);
      })
    )
  }
}

