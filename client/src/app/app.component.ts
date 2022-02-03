import { Component, OnInit } from '@angular/core';
import { User } from './core/models/user';
import { AccountService } from './core/services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  constructor(private accountService: AccountService) { }

  public ngOnInit(): void {
    this.setCurrentUser();  
  }

  private setCurrentUser(): void {
    const user: User = JSON.parse(localStorage.getItem('user'));
    if(user) {
      this.accountService.setCurrentUser(user);
    }
  }
}