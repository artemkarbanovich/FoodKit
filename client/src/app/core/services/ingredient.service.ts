import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Ingredient } from '../models/ingredient';

@Injectable({
  providedIn: 'root'
})
export class IngredientService {
  private baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public addIngredient(ingredient: Ingredient): Observable<number> {
    return this.http.post<number>(this.baseUrl + 'ingredient/add-ingredient', ingredient);
  }
}