import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FileItem, FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';
import { Dish } from 'src/app/core/models/dish';
import { DishUpdate } from 'src/app/core/models/dishUpdate';
import { Image } from 'src/app/core/models/image';
import { DishService } from 'src/app/core/services/dish.service';
import { DeletionConfirmationComponent } from 'src/app/shared/components/deletion-confirmation/deletion-confirmation.component';

@Component({
  selector: 'app-dish-edit',
  templateUrl: './dish-edit.component.html',
  styleUrls: ['./dish-edit.component.scss'],
  host: { '(window:resize)': 'stepperOrientation()' }
})
export class DishEditComponent implements OnInit {
  public dish: Dish;
  public dishForm: FormGroup;
  public uploader: FileUploader;
  private cookingTimeMinutes: number;

  constructor(private route: ActivatedRoute, private dishService: DishService, private dialog: MatDialog,
    private toastr: ToastrService, private router: Router, private formBuilder: FormBuilder) { }
  

  public ngOnInit(): void {
    this.stepperOrientation();
    this.loadDish();
    this.initializeUploader();
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
        next: (dish: Dish) => this.dish = dish,
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