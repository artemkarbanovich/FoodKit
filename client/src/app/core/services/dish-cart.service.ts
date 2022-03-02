import { Injectable } from '@angular/core';
import { DishCartItem } from '../models/dishCartItem';

@Injectable({
  providedIn: 'root'
})
export class DishCartService {
  public dishCartItems: DishCartItem[] = [];
}