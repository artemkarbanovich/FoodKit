import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../services/account.service';
import { RegistrationComponent } from '../registration/registration.component';
import { SignInComponent } from '../sign-in/sign-in.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  constructor(public accountService: AccountService, private dialog: MatDialog,
    private router: Router, private toastr: ToastrService) { }


  public openSignInDialog(): void {
    this.dialog.open(SignInComponent, { 
      autoFocus: false,
      maxWidth: '92%',
      maxHeight: '90vh'
    });
  }

  public openRegistrationDialog(): void {
    this.dialog.open(RegistrationComponent, { 
      autoFocus: false,
      maxWidth: '92%',
      maxHeight: '90vh'
    });
  }

  public signOut(): void {
    this.accountService.signOut();
    this.router.navigateByUrl('/');
    this.toastr.success('Вы успешно вышли');
  }
}
