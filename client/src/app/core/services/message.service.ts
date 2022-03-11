import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';
import { Observable } from 'rxjs';
import { MessageAdminList } from '../models/messageAdminList';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }


  public sendMessage(content: string, recipientId: number): Observable<Message> {
    return this.http.post<Message>(this.baseUrl + 'message/send-message', { content: content, recipientId: recipientId });
  }

  public getMessageThread(recipientId: number): Observable<Message[]> {
    return this.http.get<Message[]>(this.baseUrl + 'message/get-message-thread/' + recipientId);
  }

  public getSupportManagerId(): Observable<number> {
    return this.http.get<number>(this.baseUrl + 'message/get-support-manager-id');
  }

  public getMessageAdminList(): Observable<MessageAdminList[]> {
    return this.http.get<MessageAdminList[]>(this.baseUrl + 'message/get-message-admin-list');
  }
}