import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Message } from '../../models/message';
import { MessageService } from '../../services/message.service';
import { PresenceService } from '../../services/presence.service';

@Component({
  selector: 'app-user-support',
  templateUrl: './user-support.component.html',
  styleUrls: ['./user-support.component.scss']
})
export class UserSupportComponent implements OnInit {
  @ViewChild('chatWrap') private chatWrap: ElementRef;
  public supportManagerId: number;
  public messages: Message[] = [];
  public messageContent: string = '';

  constructor(private messageService: MessageService, public presenceService: PresenceService) { }

  
  public ngOnInit(): void {
    this.messageService.getSupportManagerId().subscribe(id => {
      this.supportManagerId = id;
      this.loadMessages();
    });
  }

  public sendMessage(): void {
    this.messageService.sendMessage(this.messageContent, this.supportManagerId).subscribe((message: Message) => {
      this.messageContent = '';
      this.messages.push(message);
      this.scrollChatToBottom();
    });
  }

  private loadMessages(): void {
    this.messageService.getMessageThread(this.supportManagerId).subscribe((messages: Message[]) => {
      this.messages = messages;
      this.scrollChatToBottom()
    });
  }

  private scrollChatToBottom(): void {
    setTimeout(() => this.chatWrap.nativeElement.scrollTop = this.chatWrap.nativeElement.scrollHeight, 0);
  }
}