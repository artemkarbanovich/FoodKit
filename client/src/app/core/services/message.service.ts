import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';
import { BehaviorSubject, Observable, take } from 'rxjs';
import { MessageAdminList } from '../models/messageAdminList';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Account } from '../models/account';
import { ToastrService } from 'ngx-toastr';
import { Group } from '../models/group';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private baseUrl: string = environment.apiUrl;
  private hubUrl: string = environment.hubUrl;
  private hubConnection: HubConnection;
  private messageThreadSource: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>([]);
  public messageThread$: Observable<Message[]> = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient, private toastr: ToastrService) { }


  public createHubConnection(user: Account, otherUserId: number): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?otherUserId=' + otherUserId, { accessTokenFactory: () => user.token })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch(error => {
        this.toastr.error('Ошибка подключения к серверу');
        console.log(error);
      });
    
    this.hubConnection.on('ReceiveMessageThread', (messages: Message[]) => {
      this.messageThreadSource.next(messages);
    });

    this.hubConnection.on('NewMessage', (message: Message) => {
      this.messageThread$.pipe(take(1)).subscribe((messages: Message[]) => {
        this.messageThreadSource.next([...messages, message]);
      });
    });

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if(group.connections.some(c => c.userId === otherUserId)) {
        this.messageThread$.pipe(take(1)).subscribe((messages: Message[]) => {
          messages.forEach(message => {
            if(!message.dateRead) {
              message.dateRead = new Date(Date.now());
            }
          });
          this.messageThreadSource.next([...messages]);
        });
      }
    });
  }

  public stopHubConnection(): void {
    if(this.hubConnection) {
      this.messageThreadSource.next([]);
      this.hubConnection.stop();
    }
  }
  
  public async sendMessage(content: string, recipientId: number): Promise<any> {
    return this.hubConnection.invoke('SendMessage', {recipientId: recipientId, content})
      .catch(error => {
        this.toastr.error('Ошибка отправки сообщения');
        console.log(error);
      });
  }

  public getSupportManagerId(): Observable<number> {
    return this.http.get<number>(this.baseUrl + 'message/get-support-manager-id');
  }

  public getMessageAdminList(): Observable<MessageAdminList[]> {
    return this.http.get<MessageAdminList[]>(this.baseUrl + 'message/get-message-admin-list');
  }
}