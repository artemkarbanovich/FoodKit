import { Component, OnInit } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
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
  public selectedUserDishes: UserDish[] = [];
  public displayColumns: string[] = 
    ['select', 'name', 'dishDate', 'dishWeight', 'proteins', 'fats', 'carbohydrates', 'calories'];

  constructor(private userDishService: UserDishService, private toastr: ToastrService) { }

  public ngOnInit(): void {
    this.loadUserDishes();
  }

  public deleteSelectedUserDishes(): void {
    if(this.selectedUserDishes.length > 0) {
      this.userDishService.deleteUserDishes(this.selectedUserDishes.map(sud => sud.id)).subscribe(() => {
        this.loadUserDishes();
        if(this.selectedUserDishes.length == 1) {
          this.toastr.success('Продукт успешно удален');
        } else {
          this.toastr.success('Продукты успешно удалены');
        }
        this.selectedUserDishes = [];
      });
    }
  }

  public userDishChangeEvent(clickedUserDish: UserDish, $event: MatCheckboxChange): void {
    if($event.checked && this.selectedUserDishes.findIndex(sud => sud.id === clickedUserDish.id) === -1) {
      this.selectedUserDishes.push(clickedUserDish);
    } else if(!$event.checked){
      let indexOfClickedUserDish = this.selectedUserDishes.findIndex(sud => sud.id === clickedUserDish.id);
      if(indexOfClickedUserDish !== -1) {
        this.selectedUserDishes.splice(indexOfClickedUserDish, 1);
      }
    }
  }

  public setChecked(row: UserDish): boolean {
     return (this.selectedUserDishes.findIndex(sud => sud.id === row.id) !== -1) ? true : false;
  }

  public handlePage(event?: PageEvent): PageEvent {
    this.pagination.currentPage = event.pageIndex;
    this.pagination.pageSize = event.pageSize;
    this.loadUserDishes();
    return event;
  }

  public loadUserDishes(): void {
    this.userDishService.getUserDishes(this.pagination?.currentPage + 1, this.pagination?.pageSize)
    .subscribe((paginatedResult: PaginatedResult<UserDish[]>) => { 
      this.userDishes = paginatedResult.result;
      this.pagination = paginatedResult.pagination;
      this.pagination.currentPage = this.pagination.currentPage - 1;
    });
  }
}