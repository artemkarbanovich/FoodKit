import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { take } from 'rxjs';
import { Account } from '../../models/account';
import { AccountService } from '../../services/account.service';
import { MessageService } from '../../services/message.service';
import { PresenceService } from '../../services/presence.service';

@Component({
  selector: 'app-user-support',
  templateUrl: './user-support.component.html',
  styleUrls: ['./user-support.component.scss']
})
export class UserSupportComponent implements OnInit, OnDestroy {
  @ViewChild('chatWrap') private chatWrap: ElementRef;
  public supportManagerId: number;
  public messageContent: string = '';
  private user: Account;

  constructor(public messageService: MessageService, public presenceService: PresenceService,
    private accountService: AccountService) { 
      accountService.currentUser$.pipe(take(1)).subscribe((user: Account) => this.user = user);
    }
  
  
  public ngOnInit(): void {
    this.messageService.getSupportManagerId().subscribe(id => {
      this.supportManagerId = id;
      this.messageService.createHubConnection(this.user, this.supportManagerId);
      this.messageService.messageThread$.subscribe(() => {
        this.scrollChatToBottom();
      });
    });
  }

  public ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  public sendMessage(): void {
    this.messageService.sendMessage(this.messageContent, this.supportManagerId).then(() => {
      this.messageContent = '';
      this.scrollChatToBottom();
    });
  }

  private scrollChatToBottom(): void {
    setTimeout(() => this.chatWrap.nativeElement.scrollTop = this.chatWrap.nativeElement.scrollHeight, 0);
  }
}