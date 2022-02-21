import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DishService } from 'src/app/core/services/dish.service';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { FileItem, FileUploader } from 'ng2-file-upload';
import { DishAdd } from 'src/app/core/models/dishAdd';
import { Router } from '@angular/router';
import { Ingredient } from 'src/app/core/models/ingredient';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { IngredientService } from 'src/app/core/services/ingredient.service';
import { PageEvent } from '@angular/material/paginator';
import { Pagination } from 'src/app/core/models/pagination';
import { PaginatedResult } from 'src/app/core/models/paginatedResult';
import { DishAddIngredient } from '../../../models/dishAddIngredient';

@Component({
  selector: 'app-dish-add-form',
  templateUrl: './dish-add-form.component.html',
  styleUrls: ['./dish-add-form.component.scss'],
  providers: [ { provide: STEPPER_GLOBAL_OPTIONS, useValue: {showError: true} } ],
  host: { '(window:resize)': 'stepperOrientation()' }
})
export class DishAddFormComponent implements OnInit {
  public dishForm: FormGroup;
  public displayedColumns: string[] = ['name', 'size', 'deleteAction'];
  public isVisited: boolean = false;
  public uploader: FileUploader;
  public pagination: Pagination;
  public pageEvent: PageEvent;
  public ingredients: Ingredient[] = [];
  public selectedIngredients: Ingredient[] = [];

  constructor(private dishService: DishService, private ingredientService: IngredientService,
    private formBuilder: FormBuilder, private toastr: ToastrService, private router: Router) { }
  

  public ngOnInit(): void {
    this.stepperOrientation();
    this.initializeForm();
    this.initializeUploader();
    this.loadIngredients();
  }

  public stepperOrientation(): string {
    return (window.innerWidth <= 650) ? 'vertical' : 'horizontal';
  }

  public onSelectStepper(): void {
    if(!this.isVisited) {
      this.isVisited = true;
    }
  }

  public addDish(): void {
    let dish: DishAdd = {
      name: this.dishForm.controls['name'].value,
      cookingTimeHours: Math.floor(this.dishForm.controls['cookingTime'].value / 60),
      cookingTimeMinutes: this.dishForm.controls['cookingTime'].value % 60,
      youWillNeed: this.dishForm.controls['youWillNeed'].value,
      price: this.dishForm.controls['price'].value,
      isAvailableForSingleOrder: this.dishForm.controls['isAvailableForSingleOrder'].value,
      ingredients: [],
      images: []
    };

    this.selectedIngredients.forEach((ingr: Ingredient) => {
      let ingredientToDish: DishAddIngredient = {
        id: ingr.id,
        ingredientWeightPerPortion: ingr.ingredientWeightPerPortion
      };
      dish.ingredients.push(ingredientToDish);
    });

    this.uploader.queue.forEach((img: FileItem) => dish.images.push(img._file));

    this.dishService.addDish(dish).subscribe(() => {
      this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => this.router.navigate(['dishes']));
      this.toastr.success('Блюдо успешно добавлено');
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

  public drop(event: CdkDragDrop<string[]>): void {
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

  private initializeForm(): void {
    this.dishForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(1),
        Validators.maxLength(30)]],
      cookingTime: ['', [Validators.required, Validators.min(1),
        Validators.max(1400), Validators.pattern('^[1-9][0-9]*$')]],
      youWillNeed: ['', [Validators.required, Validators.minLength(5),
        Validators.maxLength(100)]],
      price: ['', [Validators.required, Validators.min(0),
        Validators.max(300), Validators.pattern('^-?\\d*(\\.\\d+)?$')]],
      isAvailableForSingleOrder: [false, []]
    });
  }

  private initializeUploader(): void {
    this.uploader = new FileUploader({ 
      isHTML5: true,
      allowedFileType: ['image']
    });
    this.uploader.onAfterAddingFile = (file: FileItem) => file.withCredentials = false;
  }
}