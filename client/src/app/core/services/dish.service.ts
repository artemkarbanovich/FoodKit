import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DishAdd } from '../models/dishAdd';
import { DishAddIngredient } from '../models/dishAddIngredient';

@Injectable({
  providedIn: 'root'
})
export class DishService {
  private baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  
  public addDish(dish: DishAdd): Observable<Object> {
    let formData = new FormData();

    formData.append('name', dish.name);
    formData.append('cookingTimeHours', dish.cookingTimeHours.toString());
    formData.append('cookingTimeMinutes', dish.cookingTimeMinutes.toString());
    formData.append('youWillNeed', dish.youWillNeed);
    formData.append('price', dish.price.toString());
    formData.append('isAvailableForSingleOrder', dish.isAvailableForSingleOrder.toString());

    dish.ingredients.forEach((ingr: DishAddIngredient, index: number = 0) => {
      formData.append('ingredients[' + index + '].Id', ingr.id.toString());
      formData.append('ingredients[' + index++ + '].IngredientWeightPerPortion', ingr.ingredientWeightPerPortion.toString());
    });

    dish.images.forEach((img: File) => {
      formData.append('Images', img);
    });

    return this.http.post(this.baseUrl + 'dish/add-dish', formData);
  }
}
