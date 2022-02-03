import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Register } from '../models/register';
import { SignIn } from '../models/signIn';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl: string = environment.apiUrl;
  private currentUserSource: ReplaySubject<User> = new ReplaySubject<User>(1);
  public currentUser$: Observable<User> = this.currentUserSource.asObservable();

  
  constructor(private http: HttpClient) { }


  public register(user: Register): Observable<void> {
    return this.http.post(this.baseUrl + 'account/register', user).pipe(
      map((user: User) => {
        if(user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  public signIn(user: SignIn): Observable<void> {
    return this.http.post(this.baseUrl + 'account/sign-in', user).pipe(
      map((user: User) => {
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

  public setCurrentUser(user: User): void {
    user.roles = [];
    const roles = JSON.parse(atob(user.token.split('.')[1])).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
}