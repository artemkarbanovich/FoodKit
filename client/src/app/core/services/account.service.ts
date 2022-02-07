import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Register } from '../models/register';
import { SignIn } from '../models/signIn';
import { Account } from '../models/account';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl: string = environment.apiUrl;
  private currentUserSource: ReplaySubject<Account> = new ReplaySubject<Account>(1);
  public currentUser$: Observable<Account> = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }


  public register(user: Register): Observable<void> {
    return this.http.post(this.baseUrl + 'account/register', user).pipe(
      map((user: Account) => {
        if(user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  public signIn(user: SignIn): Observable<void> {
    return this.http.post(this.baseUrl + 'account/sign-in', user).pipe(
      map((user: Account) => {
        if(user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  public signOut(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  public setCurrentUser(user: Account): void {
    user.roles = [];
    const roles = JSON.parse(atob(user.token.split('.')[1])).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  public updateUserAfterChange(name: string, userName: string, phoneNumber: string, email: string) {
    let user: Account = JSON.parse(localStorage.getItem('user'));
    if(user) {
      user.name = name;
      user.userName = userName;
      user.phoneNumber = phoneNumber;
      user.email = email;
      this.setCurrentUser(user);
    }
  }
}