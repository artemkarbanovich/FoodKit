import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { PaginatedResult } from '../../models/paginatedResult';
import { Pagination } from '../../models/pagination';
import { UserDish } from '../../models/userDish';
import { UserDishService } from '../../services/user-dish.service';

@Component({
  selector: 'app-user-dish-table',
  templateUrl: './user-dish-table.component.html',
  styleUrls: ['./user-dish-table.component.scss']
})
export class UserDishTableComponent implements OnInit {
  public pagination: Pagination;
  public pageEvent: PageEvent;
  public userDishes: UserDish[];
  public displayColumns: string[] = 
    ['name', 'dishDate', 'dishWeight', 'proteins', 'fats', 'carbohydrates', 'calories'];

  constructor(private userDishService: UserDishService) { }

  public ngOnInit(): void {
    this.loadUserDishes();
  }

  public handlePage(event?: PageEvent): PageEvent {
    this.pagination.currentPage = event.pageIndex;
    this.pagination.pageSize = event.pageSize;
    this.loadUserDishes();
    return event;
  }

  private loadUserDishes(): void {
    this.userDishService.getUserDishes(this.pagination?.currentPage + 1, this.pagination?.pageSize)
    .subscribe((paginatedResult: PaginatedResult<UserDish[]>) => { 
      this.userDishes = paginatedResult.result;
      this.pagination = paginatedResult.pagination;
      this.pagination.currentPage = this.pagination.currentPage - 1;
    });
  }
}