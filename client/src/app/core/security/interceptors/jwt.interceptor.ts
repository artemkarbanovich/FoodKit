import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { Account } from '../../models/account';
import { AccountService } from '../../services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private accountService: AccountService) { }

  public intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: Account;
    this.accountService.currentUser$.pipe(take(1)).subscribe((user: Account) => currentUser = user);

    if(currentUser) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${ currentUser.token }`
        }
      });
    }
    return next.handle(request);
  }
}
