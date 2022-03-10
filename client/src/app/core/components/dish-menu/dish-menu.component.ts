import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Dish } from '../../models/dish';
import { PaginatedResult } from '../../models/paginatedResult';
import { Pagination } from '../../models/pagination';
import { DishCartService } from '../../services/dish-cart.service';
import { DishService } from '../../services/dish.service';
import { Ingredient } from '../../models/ingredient';
import { DishCartItem } from '../../models/dishCartItem';

@Component({
  selector: 'app-dish-menu',
  templateUrl: './dish-menu.component.html',
  styleUrls: ['./dish-menu.component.scss']
})
export class DishMenuComponent implements OnInit {
  public pagination: Pagination;
  public pageEvent: PageEvent;
  public dishes: Dish[] = [];
  public numberOfPersons: string = '2';

  constructor(private dishService: DishService, private dishCartService: DishCartService) { }


  public ngOnInit(): void {
    this.loadDishes();
  }

  public getWeightOfDish(dish: Dish): number {
    let singleDishWeight: number = 0;
    dish.ingredients.forEach(ingr => {
      singleDishWeight += ingr?.ingredientWeightPerPortion;
    });
    return singleDishWeight * Number(this.numberOfPersons);
  }

  public getAverageValue(ingredients: Ingredient[], property: string): number {
    let sum = 0;
    ingredients.forEach(ingr => {
      sum += ingr[property];
    });
    return Math.round(sum / ingredients.length);
  }

  public addToCart(dish: Dish): void {
    const dishCartItem: DishCartItem = {
      dishId: dish.id,
      dishName: dish.name,
      dishPrice: dish.price,
      dishImageUrl: dish.images[0].url,
      count: 1,
      numberOfPersons: Number(this.numberOfPersons)
    };

    const checkItemId = this.dishCartService.dishCartItems.findIndex(item => item.dishId === dishCartItem.dishId &&
      item.numberOfPersons === dishCartItem.numberOfPersons);

    if(checkItemId !== -1) {
      this.dishCartService.dishCartItems[checkItemId].count++;
    } else {
      this.dishCartService.dishCartItems.push(dishCartItem);
    }
  }

  public handlePage(event?: PageEvent): PageEvent {
    this.pagination.currentPage = event.pageIndex;
    this.pagination.pageSize = event.pageSize;
    this.loadDishes();
    return event;
  }

  private loadDishes(): void {
    this.dishService.getDishesUserList(this.pagination?.currentPage + 1, this.pagination?.pageSize)
    .subscribe((paginatedResult: PaginatedResult<Dish[]>) => {
      this.dishes = paginatedResult.result;
      this.pagination = paginatedResult.pagination;
      this.pagination.currentPage = this.pagination.currentPage - 1;
    });
  }
}