import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { DishCartService } from '../../services/dish-cart.service';

@Injectable({
  providedIn: 'root'
})
export class OrderGuard implements CanActivate {
  constructor(private dishCartService: DishCartService) { }

  public canActivate(): boolean {
    if(this.dishCartService.dishCartItems.length === 0)
      return false;
    else
      return true;
  }
}
