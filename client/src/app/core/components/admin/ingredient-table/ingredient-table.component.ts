import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { Ingredient } from 'src/app/core/models/ingredient';
import { PaginatedResult } from 'src/app/core/models/paginatedResult';
import { Pagination } from 'src/app/core/models/pagination';
import { IngredientService } from 'src/app/core/services/ingredient.service';
import { DeletionConfirmationComponent } from 'src/app/shared/components/deletion-confirmation/deletion-confirmation.component';

@Component({
  selector: 'app-ingredient-table',
  templateUrl: './ingredient-table.component.html',
  styleUrls: ['./ingredient-table.component.scss']
})
export class IngredientTableComponent implements OnInit {
  public pagination: Pagination;
  public pageEvent: PageEvent;
  public ingredients: Ingredient[];
  public displayColumns: string[] = 
    ['name', 'proteins', 'fats', 'carbohydrates', 'calories', 'delete'];

  constructor(private ingredientService: IngredientService, private toast: ToastrService,
    private dialog: MatDialog) { }


  public ngOnInit(): void {
    this.loadIngredients();
  }

  public deleteIngredient(ingredient: Ingredient): void {
    this.dialog.open(DeletionConfirmationComponent, {disableClose: true, autoFocus: false})
    .afterClosed().subscribe(result => {
      if(result == false) return;
      
      this.ingredientService.deleteIngredient(ingredient.id).subscribe(() => {
        this.loadIngredients();
        this.toast.success('Ингредиент успешно удален');
      });
    });
  }

  public handlePage(event?: PageEvent): PageEvent {
    this.pagination.currentPage = event.pageIndex;
    this.pagination.pageSize = event.pageSize;
    this.loadIngredients();
    return event;
  }

  public loadIngredients(): void {
    this.ingredientService.getIngredients(this.pagination?.currentPage + 1, this.pagination?.pageSize)
    .subscribe((paginatedResult: PaginatedResult<Ingredient[]>) => {
      this.ingredients = paginatedResult.result;
      this.pagination = paginatedResult.pagination;
      this.pagination.currentPage = this.pagination.currentPage - 1;
    });
  }
}