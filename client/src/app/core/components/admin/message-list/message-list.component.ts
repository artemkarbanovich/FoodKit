import { Component, OnInit } from '@angular/core';
import { MessageAdminList } from 'src/app/core/models/messageAdminList';
import { MessageService } from 'src/app/core/services/message.service';

@Component({
  selector: 'app-message-list',
  templateUrl: './message-list.component.html',
  styleUrls: ['./message-list.component.scss']
})
export class MessageListComponent implements OnInit {
  public messageList: MessageAdminList[] = [];

  constructor(private messageService: MessageService) { }

  public ngOnInit(): void {
    this.messageService.getMessageAdminList().subscribe((messageList: MessageAdminList[]) => {
      this.messageList = messageList;
    });
  }
}