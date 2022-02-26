import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { DishAdminList } from 'src/app/core/models/dishAdminList';
import { PaginatedResult } from 'src/app/core/models/paginatedResult';
import { Pagination } from 'src/app/core/models/pagination';
import { DishService } from 'src/app/core/services/dish.service';

@Component({
  selector: 'app-dish-list',
  templateUrl: './dish-list.component.html',
  styleUrls: ['./dish-list.component.scss']
})
export class DishListComponent implements OnInit {
  public pagination: Pagination;
  public pageEvent: PageEvent;
  public dishesAdminList: DishAdminList[] = [];

  constructor(private dishService: DishService) { }


  public ngOnInit(): void {
    this.loadDishesAdminList();
  }

  public handlePage(event?: PageEvent): PageEvent {
    this.pagination.currentPage = event.pageIndex;
    this.pagination.pageSize = event.pageSize;
    this.loadDishesAdminList();
    return event;
  }

  public loadDishesAdminList(): void {
    this.dishService.getDishesAdminList(this.pagination?.currentPage + 1, this.pagination?.pageSize)
    .subscribe((paginatedResult: PaginatedResult<DishAdminList[]>) => {
      this.dishesAdminList = paginatedResult.result;
      this.pagination = paginatedResult.pagination;
      this.pagination.currentPage = this.pagination.currentPage - 1;
      console.log(this.dishesAdminList);
    });
  }
}