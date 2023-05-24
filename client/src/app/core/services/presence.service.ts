import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, take } from 'rxjs';
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
      .configureLogging(LogLevel.None)
      .build();

    this.hubConnection
      .start()
      .catch(error => {
        this.toastr.error('Ошибка подключения к серверу');
        console.log(error);
      });

    this.hubConnection.on('UserIsOnline', (userId: number) => {
      this.onlineUsers$.pipe(take(1)).subscribe((userIds: number[]) => {
        this.onlineUsersSource.next([...userIds, userId]);
      });
    });

    this.hubConnection.on('UserIsOffline', (userId: number) => {
      this.onlineUsers$.pipe(take(1)).subscribe((userIds: number[]) => {
        this.onlineUsersSource.next([...userIds.filter(u => u !== userId)]);
      });
    });

    this.hubConnection.on('GetOnlineUsers', (userIds: number[]) => {
      this.onlineUsersSource.next(userIds);
    });

    this.hubConnection.on('NewMessageReceived', ({senderName, content}) => {
      this.toastr.info(content, senderName);
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