import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { DishCartItem } from '../../models/dishCartItem';
import { AccountService } from '../../services/account.service';
import { DishCartService } from '../../services/dish-cart.service';
import { RegistrationComponent } from '../registration/registration.component';
import { SignInComponent } from '../sign-in/sign-in.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  constructor(public accountService: AccountService, private dialog: MatDialog,
    private router: Router, private toastr: ToastrService, public dishCartService: DishCartService) { }


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
    this.toastr.success('You successful logout');
  }

  public deleteCartItem(cartItem: DishCartItem): void {
    let checkItemId = this.dishCartService.dishCartItems.findIndex(item => item.dishId === cartItem.dishId &&
      item.numberOfPersons === cartItem.numberOfPersons);

    if(checkItemId !== -1 && this.dishCartService.dishCartItems[checkItemId].count > 1) {
      this.dishCartService.dishCartItems[checkItemId].count--;
    } else if (checkItemId !== -1) {
      this.dishCartService.dishCartItems.splice(checkItemId, 1);
    }
  }

  public getItemsCount(): number {
    let sum: number = 0;

    this.dishCartService.dishCartItems.forEach(item => {
      sum += item.count;
    });

    return sum;
  }

  public getOrderSummary(): number {
    let sum: number = 0;

    this.dishCartService.dishCartItems.forEach(item => {
      sum += item.dishPrice * item.count * item.numberOfPersons;
    });

    return sum;
  }
}