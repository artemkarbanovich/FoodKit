import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { take } from 'rxjs';
import { Account } from 'src/app/core/models/account';
import { AccountService } from 'src/app/core/services/account.service';
import { MessageService } from 'src/app/core/services/message.service';
import { PresenceService } from 'src/app/core/services/presence.service';

@Component({
  selector: 'app-admin-chat',
  templateUrl: './admin-chat.component.html',
  styleUrls: ['./admin-chat.component.scss']
})
export class AdminChatComponent implements OnInit, OnDestroy {
  @ViewChild('chatWrap') private chatWrap: ElementRef;
  public recipientId: number;
  public messageContent: string = '';
  private user: Account;
  
  constructor(private messageService: MessageService, private route: ActivatedRoute,
    public presenceService: PresenceService, private accountService: AccountService) { 
      accountService.currentUser$.pipe(take(1)).subscribe((user: Account) => this.user = user);
    }
  

  public ngOnInit(): void {
    this.loadMessages();
  }

  public ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  public sendMessage(): void {
    this.messageService.sendMessage(this.messageContent, this.recipientId).then(() => {
      this.messageContent = '';
      this.scrollChatToBottom();
    });
  }

  private loadMessages(): void {
    this.route.params.subscribe((params: Params) => {
      this.recipientId = Number(params['id']);
      this.messageService.createHubConnection(this.user, this.recipientId);
      this.messageService.messageThread$.subscribe(() => {
        this.scrollChatToBottom();
      });
    });
  }

  private scrollChatToBottom(): void {
    setTimeout(() => this.chatWrap.nativeElement.scrollTop = this.chatWrap.nativeElement.scrollHeight, 0);
  }
}