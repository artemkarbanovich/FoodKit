import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DishAdd } from '../models/dishAdd';
import { DishAddIngredient } from '../models/dishAddIngredient';
import { DishAdminList } from '../models/dishAdminList';
import { PaginatedResult } from '../models/paginatedResult';

@Injectable({
  providedIn: 'root'
})
export class DishService {
  private baseUrl: string = environment.apiUrl;
  public paginatedResult: PaginatedResult<DishAdminList[]> = new PaginatedResult<DishAdminList[]>();

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

  public getDishesAdminList(currentPage?: number, pageSize?: number): Observable<PaginatedResult<DishAdminList[]>> {
    let params = new HttpParams();
    
    if(currentPage != null && pageSize != null) {
      params = params.append('currentPage', currentPage.toString());
      params = params.append('pageSize', pageSize.toString());
    }

    return this.http.get<DishAdminList[]>(this.baseUrl + 'dish/get-dishes-admin-list',  {observe: 'response' , params})
    .pipe(
      map((response: HttpResponse<DishAdminList[]>) => {
        this.paginatedResult.result = response.body;
        if(response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return this.paginatedResult;
      })
    )
  }
}