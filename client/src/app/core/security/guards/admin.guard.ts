import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { map, Observable } from 'rxjs';
import { Account } from '../../models/account';
import { AccountService } from '../../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private accountService: AccountService) { }

  public canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map((user:  Account) => {
        if(user.roles.includes('Admin'))
          return true;
        return false;
      })
    )
  }
}
