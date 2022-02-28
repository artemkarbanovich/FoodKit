import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FileItem, FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';
import { Dish } from 'src/app/core/models/dish';
import { DishAddIngredient } from 'src/app/core/models/dishAddIngredient';
import { DishUpdate } from 'src/app/core/models/dishUpdate';
import { Image } from 'src/app/core/models/image';
import { Ingredient } from 'src/app/core/models/ingredient';
import { PaginatedResult } from 'src/app/core/models/paginatedResult';
import { Pagination } from 'src/app/core/models/pagination';
import { DishService } from 'src/app/core/services/dish.service';
import { IngredientService } from 'src/app/core/services/ingredient.service';
import { DeletionConfirmationComponent } from 'src/app/shared/components/deletion-confirmation/deletion-confirmation.component';

@Component({
  selector: 'app-dish-edit',
  templateUrl: './dish-edit.component.html',
  styleUrls: ['./dish-edit.component.scss'],
  host: { '(window:resize)': 'stepperOrientation()' }
})
export class DishEditComponent implements OnInit {
  private cookingTimeMinutes: number;
  public dish: Dish;
  public dishForm: FormGroup;
  public uploader: FileUploader;
  public pagination: Pagination;
  public pageEvent: PageEvent;
  public ingredients: Ingredient[] = [];
  public selectedIngredients: Ingredient[] = [];
  public errors: string[] = [];

  constructor(private route: ActivatedRoute, private dishService: DishService, private dialog: MatDialog,
    private toastr: ToastrService, private router: Router, private formBuilder: FormBuilder,
    private ingredientService: IngredientService) { }
  

  public ngOnInit(): void {
    this.stepperOrientation();
    this.loadDish();
    this.initializeUploader();
    this.loadIngredients();
  }

  public stepperOrientation(): string {
    return (window.innerWidth <= 780) ? 'vertical' : 'horizontal';
  }

  public saveDishFormChanges(): void {
    let dishUpdate: DishUpdate = {
      id: this.dish.id,
      name: this.dishForm.controls['name'].value,
      cookingTimeHours: Math.floor(this.dishForm.controls['cookingTime'].value / 60),
      cookingTimeMinutes: this.dishForm.controls['cookingTime'].value % 60,
      youWillNeed: this.dishForm.controls['youWillNeed'].value,
      price: this.dishForm.controls['price'].value,
      isAvailableForSingleOrder: this.dishForm.controls['isAvailableForSingleOrder'].value
    };

    this.dishService.updateDish(dishUpdate).subscribe(() => {
      this.dishForm.markAsPristine();
      this.toastr.success('Изменения успешно сохранены');
    });
  }

  public deleteDishImage(image: Image): void {
    if(this.dish.images.length === 0) {
      return;
    }

    this.dialog.open(DeletionConfirmationComponent, {disableClose: true, autoFocus: false})
    .afterClosed().subscribe(result => {
      if(result == false) return;

      this.dishService.deleteDishImage(image.id).subscribe(() => {
        this.dish.images.splice(this.dish.images.indexOf(image), 1)
        this.toastr.success('Изображение успешно удалено');
      });
    });
  }

  public addImages(): void {
    let imageFiles: File[] = [];
    this.uploader.queue.forEach((img: FileItem) => imageFiles.push(img._file));

    this.dishService.addImages(imageFiles, this.dish.id).subscribe((images: Image[]) => {
      this.uploader.queue = [];
      this.dish.images = this.dish.images.concat(images);

      if(imageFiles.length === 1) {
        this.toastr.success('Изображение успешно добавлено');
      } else {
        this.toastr.success('Изображения успешно добавлены');
      }
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
      this.ingredients = [];
      paginatedResult.result.forEach((ingr: Ingredient) => {
        if(this.selectedIngredients.findIndex(si => si.id === ingr.id) === -1) {
          ingr.ingredientWeightPerPortion = null;
          this.ingredients.push(ingr);
        }
      });
      this.pagination = paginatedResult.pagination;
      this.pagination.currentPage = this.pagination.currentPage - 1;
    });
  }

  public dropIngredient(event: CdkDragDrop<Ingredient[]>): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    }
  }

  public updateDishIngredients(): void {
    this.errors = [];

    if(this.selectedIngredients.length === 0) {
      this.errors.push("Вы должны добавить хотя бы один ингредиент");
    }
    this.selectedIngredients.forEach((si: Ingredient) => {
      if(this.errors.indexOf("Масса ингредиента не может быть пустой или превышать 350 грамм") === -1 && 
        si.ingredientWeightPerPortion === null || si.ingredientWeightPerPortion > 350) {
          this.errors.push("Масса ингредиента не может быть пустой или превышать 350 грамм");
      } if(si.ingredientWeightPerPortion !== null && 
          this.errors.indexOf("Масса ингредиента должна быть целым значением") === -1 && 
          si.ingredientWeightPerPortion?.toString().includes('.') ||
          si.ingredientWeightPerPortion?.toString().includes(',') || 
          si.ingredientWeightPerPortion?.toString().includes('e')) {
            this.errors.push("Масса ингредиента должна быть целым значением");
        }
    });

    if(this.errors.length !== 0) {
      return;
    }

    let selectedDishIngredients: DishAddIngredient[] = [];

    this.selectedIngredients.forEach((ingr: Ingredient) => {
      let ingredientToDish: DishAddIngredient = {
        id: ingr.id,
        ingredientWeightPerPortion: ingr.ingredientWeightPerPortion
      };
      selectedDishIngredients.push(ingredientToDish);
    });

    this.dishService.updateDishIngredients(selectedDishIngredients, this.dish.id).subscribe(() => {
      this.toastr.success('Ингредиенты успешно обновлены');
    });
  }

  private loadDish(): void {
    this.route.params.subscribe((params: Params) => {
      this.dishService.getDish(params['id'])
      .pipe(
        catchError(error => {
          if(error) {
            this.toastr.error('Такого блюда не существует');
            this.router.navigateByUrl('/home');
          }
          return throwError(() => error);
        })
      )
      .subscribe({
        next: (dish: Dish) => {
          this.dish = dish;
          this.selectedIngredients = dish.ingredients;
        },
        complete: () => this.initializeForm(),
        error: (error) => console.log(error)
      });
   });
  }

  private initializeForm(): void {
    this.cookingTimeMinutes = this.dish.cookingTimeHours * 60 + this.dish.cookingTimeMinutes;

    this.dishForm = this.formBuilder.group({
      name: [this.dish.name, [Validators.required, Validators.minLength(1),
        Validators.maxLength(30)]],
      cookingTime: [this.cookingTimeMinutes, [Validators.required, Validators.min(1),
        Validators.max(1400), Validators.pattern('^[1-9][0-9]*$')]],
      youWillNeed: [this.dish.youWillNeed, [Validators.required, Validators.minLength(5),
        Validators.maxLength(100)]],
      price: [this.dish.price, [Validators.required, Validators.min(0),
        Validators.max(300), Validators.pattern('^-?\\d*(\\.\\d+)?$')]],
      isAvailableForSingleOrder: [this.dish.isAvailableForSingleOrder, []]
    });
  }
  
  private initializeUploader(): void {
    this.uploader = new FileUploader({ 
      isHTML5: true,
      allowedFileType: ['image']
    });
  }
}