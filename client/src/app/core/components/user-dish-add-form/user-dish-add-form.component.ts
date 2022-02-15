import { DatePipe } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserDish } from '../../models/userDish';
import { UserDishService } from '../../services/user-dish.service';

@Component({
  selector: 'app-user-dish-add-form',
  templateUrl: './user-dish-add-form.component.html',
  styleUrls: ['./user-dish-add-form.component.scss']
})
export class UserDishAddFormComponent implements OnInit {
  @Output() updateUserDishTable = new EventEmitter();
  public userDishForm: FormGroup;
  public minDishDate: Date = new Date(new Date().getFullYear() - 1, 0, 1);
  public maxDishDate: Date = new Date();
  
  constructor(private userDishService: UserDishService, private toastr: ToastrService,
    private formBuilder: FormBuilder, private datePipe: DatePipe) { }


  public ngOnInit(): void {
    this.initializeForm();
  }

  public addUserDish(editForm: FormGroupDirective): void {
    const userDish: UserDish = {
      name: this.userDishForm.controls['name'].value,
      dishDate: this.datePipe.transform(new Date(this.userDishForm.controls['dishDate'].value), 'yyyy-MM-dd'),
      dishWeight: this.userDishForm.controls['dishWeight'].value,
      proteins: this.userDishForm.controls['proteins'].value,
      fats: this.userDishForm.controls['fats'].value,
      carbohydrates: this.userDishForm.controls['carbohydrates'].value,
      calories: this.userDishForm.controls['calories'].value
    };

    this.userDishService.addUserDishes(Array.of(userDish)).subscribe(() => {
      this.updateUserDishTable.emit();
      this.toastr.success('Продукт упешно добавлен');
      editForm.resetForm();
      this.userDishForm.reset();
    });
  }
  
  private initializeForm(): void {
    this.userDishForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(30)]],
      dishDate: ['', [Validators.required]],
      dishWeight: ['', [Validators.required, Validators.pattern('^[1-9][0-9]*$'),
        Validators.min(1), Validators.max(5000)]],
      proteins: ['', [Validators.required, Validators.pattern('^-?\\d*(\\.\\d+)?$'),
        Validators.min(0), Validators.max(200)]],
      fats: ['', [Validators.required, Validators.pattern('^-?\\d*(\\.\\d+)?$'),
        Validators.min(0), Validators.max(200)]],
      carbohydrates: ['', [Validators.required, Validators.pattern('^-?\\d*(\\.\\d+)?$'),
        Validators.min(0), Validators.max(200)]],
      calories: ['', [Validators.required, Validators.pattern('^[1-9][0-9]*$'),
        Validators.min(1), Validators.max(1100)]]
    });
  }
}