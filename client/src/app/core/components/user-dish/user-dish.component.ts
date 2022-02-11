import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserDish } from '../../models/userDish';
import { UserDishService } from '../../services/user-dish.service';

@Component({
  selector: 'app-user-dish',
  templateUrl: './user-dish.component.html',
  styleUrls: ['./user-dish.component.scss']
})
export class UserDishComponent implements OnInit {
  public userDishForm: FormGroup;
  public minDishDate: Date = new Date(new Date().getFullYear() - 1, 0, 1);
  public maxDishDate: Date = new Date();

  constructor(private userDishService: UserDishService, private toastr: ToastrService,
    private formBuilder: FormBuilder, private datePipe: DatePipe) { }


  public ngOnInit(): void {
    this.initializeForm();
  }

  public addUserDish(editForm: FormGroupDirective): void {
    let userDish: UserDish = {
      name: this.userDishForm.controls['name'].value,
      dishDate: this.datePipe.transform(new Date(this.userDishForm.controls['dishDate'].value), 'yyyy-MM-dd'),
      dishWeight: this.userDishForm.controls['dishWeight'].value,
      proteins: this.userDishForm.controls['proteins'].value,
      fats: this.userDishForm.controls['fats'].value,
      carbohydrates: this.userDishForm.controls['carbohydrates'].value,
      calories: this.userDishForm.controls['calories'].value
    };

    this.userDishService.addUserDish(userDish).subscribe((userDishId: number) => {
      userDish.id = userDishId;
      //TODO: add to userDish[] and dynamicly display in table
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
      proteins: ['', [Validators.required, Validators.pattern('^\\d*\\.?\\d*$'),
        Validators.min(0), Validators.max(200)]],
      fats: ['', [Validators.required, Validators.pattern('^\\d*\\.?\\d*$'),
        Validators.min(0), Validators.max(200)]],
      carbohydrates: ['', [Validators.required, Validators.pattern('^\\d*\\.?\\d*$'),
        Validators.min(0), Validators.max(200)]],
      calories: ['', [Validators.required, Validators.pattern('^[1-9][0-9]*$'),
        Validators.min(1), Validators.max(1100)]]
    });
  }
}