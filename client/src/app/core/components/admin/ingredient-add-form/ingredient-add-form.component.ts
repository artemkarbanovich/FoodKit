import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';
import { Ingredient } from 'src/app/core/models/ingredient';
import { IngredientService } from 'src/app/core/services/ingredient.service';

@Component({
  selector: 'app-ingredient-add-form',
  templateUrl: './ingredient-add-form.component.html',
  styleUrls: ['./ingredient-add-form.component.scss']
})
export class IngredientAddFormComponent implements OnInit {
  @Output() updateIngredientTable = new EventEmitter();
  public ingredientForm: FormGroup;
  public errors: string[] | null = null;

  constructor(private ingredientService: IngredientService, private toastr: ToastrService,
    private formBuilder: FormBuilder) { }
  
  
  public ngOnInit(): void {
    this.initializeForm();
  }
  
  public addIngredient(editForm: FormGroupDirective, addAnyway: boolean): void {
    const ingredient: Ingredient = {
      name: this.ingredientForm.controls['name'].value,
      proteins: this.ingredientForm.controls['proteins'].value,
      fats: this.ingredientForm.controls['fats'].value,
      carbohydrates: this.ingredientForm.controls['carbohydrates'].value,
      calories: this.ingredientForm.controls['calories'].value
    };

    this.ingredientService.addIngredient(ingredient, addAnyway)
    .pipe(
      catchError(error => {
        if(error) {
          this.errors = [];
          if(error.error.errors) {
            for(const key in error.error.errors) {
              this.errors.push(error.error.errors[key]);
            }
          } else {
            this.errors.push(error.error);
          }
        }
        return throwError(() => error);
      })
    )
    .subscribe({
      error: (error) => console.log(error),
      complete: () => {
        this.updateIngredientTable.emit();
        this.toastr.success('Продукт упешно добавлен');
        editForm.resetForm();
        this.ingredientForm.reset();
      }
    });
  }

  private initializeForm(): void {
    this.ingredientForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(25)]],
      proteins: ['', [Validators.required, Validators.pattern('^-?\\d*(\\.\\d+)?$'),
        Validators.min(0), Validators.max(200)]],
      fats: ['', [Validators.required, Validators.pattern('^-?\\d*(\\.\\d+)?$'),
        Validators.min(0), Validators.max(200)]],
      carbohydrates: ['', [Validators.required, Validators.pattern('^-?\\d*(\\.\\d+)?$'),
        Validators.min(0), Validators.max(200)]],
      calories: ['', [Validators.required, Validators.pattern('^[1-9][0-9]*$'),
        Validators.min(1), Validators.max(1100)]]
    });

    this.ingredientForm.controls['name'].valueChanges.subscribe(() => {
      if(this.errors) {
        this.errors = null;
      }
    });
  }
}