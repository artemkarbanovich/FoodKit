import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate } from '@angular/router';
import { map, Observable } from 'rxjs';
import { MessageAdminList } from '../../models/messageAdminList';
import { MessageService } from '../../services/message.service';

@Injectable({
  providedIn: 'root'
})
export class ChatGuard implements CanActivate {
  constructor(private messageService: MessageService) { }
  
  public canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.messageService.getMessageAdminList().pipe(
      map((messageAdminList: MessageAdminList[]) => {
        if(messageAdminList.map((id) => id.recipientId).includes(Number(route.params['id'])))
          return true;
        else
          return false;
      })
    )
  }
}
