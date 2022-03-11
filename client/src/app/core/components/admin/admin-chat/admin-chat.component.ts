import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Message } from 'src/app/core/models/message';
import { MessageService } from 'src/app/core/services/message.service';
import { PresenceService } from 'src/app/core/services/presence.service';

@Component({
  selector: 'app-admin-chat',
  templateUrl: './admin-chat.component.html',
  styleUrls: ['./admin-chat.component.scss']
})
export class AdminChatComponent implements OnInit {
  @ViewChild('chatWrap') private chatWrap: ElementRef;
  public recipientId: number;
  public messages: Message[] = [];
  public messageContent: string = '';

  constructor(private messageService: MessageService, private route: ActivatedRoute,
    public presenceService: PresenceService) { }
  

  public ngOnInit(): void {
    this.loadMessages();
  }

  public sendMessage(): void {
    this.messageService.sendMessage(this.messageContent, this.recipientId).subscribe((message: Message) => {
      this.messageContent = '';
      this.messages.push(message);
      this.scrollChatToBottom();
    });
  }

  private loadMessages(): void {
    this.route.params.subscribe((params: Params) => {
      this.recipientId = Number(params['id']);
      this.messageService.getMessageThread(this.recipientId).subscribe((messages: Message[]) => {
        this.messages = messages;
        this.scrollChatToBottom();
      });
    });
  }

  private scrollChatToBottom(): void {
    setTimeout(() => this.chatWrap.nativeElement.scrollTop = this.chatWrap.nativeElement.scrollHeight, 0);
  }
}