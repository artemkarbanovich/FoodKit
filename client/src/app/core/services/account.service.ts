import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Account } from '../models/account';
import { PresenceService } from './presence.service';
import { StegomasterRequest } from 'src/app/stegomaster/DTOs/stegomaster-request';
import { StegomasterService } from 'src/app/stegomaster/stegomaster.service';
import { StegomasterResponse } from 'src/app/stegomaster/DTOs/stegomaster-response';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl: string = environment.apiUrl;
  private currentUserSource: ReplaySubject<Account> = new ReplaySubject<Account>(1);
  public currentUser$: Observable<Account> = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presenceService: PresenceService,
    private stegomasterService: StegomasterService) { }


  public register(stegomasterRequest: StegomasterRequest): Observable<void> {
    return this.http.post(this.baseUrl + 'account/register', stegomasterRequest).pipe(
      map((stegomasterResponse: StegomasterResponse) => {
        if(stegomasterResponse.data) {
          const image = this.stegomasterService.loadImage(stegomasterResponse.data);
          image.onload = () => {
            const userData = this.stegomasterService.processResponse(image);
            const user: Account = {
              userName: userData[0],
              name: userData[1],
              phoneNumber: userData[2],
              email: userData[3],
              token: userData[4],
            };

            this.setCurrentUser(user);
            this.presenceService.createHubConnection(user);
          }
        }
      })
    );
  }

  public signIn(stegomasterRequest: StegomasterRequest): Observable<void> {
    return this.http.post(this.baseUrl + 'account/sign-in', stegomasterRequest).pipe(
      map((stegomasterResponse: StegomasterResponse) => {
        if(stegomasterResponse.data) {
          const image = this.stegomasterService.loadImage(stegomasterResponse.data);
          image.onload = () => {
            const userData = this.stegomasterService.processResponse(image);
            const user: Account = {
              userName: userData[0],
              name: userData[1],
              phoneNumber: userData[2],
              email: userData[3],
              token: userData[4],
            };

            this.setCurrentUser(user);
            this.presenceService.createHubConnection(user);
          }
        }
      })
    );
  }

  public signOut(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presenceService.stopHubConnection();
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