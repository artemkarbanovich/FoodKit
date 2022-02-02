import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RegistrationComponent } from '../registration/registration.component';
import { SignInComponent } from '../sign-in/sign-in.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  constructor(public dialog: MatDialog) { }

  public openSignInDialog() {
    this.dialog.open(SignInComponent, { 
      autoFocus: false,
      maxWidth: '92%',
      maxHeight: '90vh'
    });
  }

  public openRegistrationDialog() {
    this.dialog.open(RegistrationComponent, { 
      autoFocus: false,
      maxWidth: '92%',
      maxHeight: '90vh'
    });
  }
}
