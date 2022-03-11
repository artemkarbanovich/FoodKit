import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Account } from '../models/account';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  private hubUrl: string = environment.hubUrl;
  private hubConnection: HubConnection;
  private onlineUsersSource: BehaviorSubject<number[]> = new BehaviorSubject<number[]>([]);
  public onlineUsers$ = this.onlineUsersSource.asObservable();

  constructor(private toastr: ToastrService) { }

  
  public createHubConnection(user: Account): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', { accessTokenFactory: () => user.token })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch(error => {
        this.toastr.error('Ошибка подключения к серверу');
        console.log(error);
      });

    this.hubConnection.on('GetOnlineUsers', (userIds: number[]) => {
      this.onlineUsersSource.next(userIds);
    });
  }

  public stopHubConnection(): void {
    this.hubConnection
      .stop()
      .catch(error => {
        this.toastr.error('Ошибка отключения от сервера');
        console.log(error);
      });
  }
}