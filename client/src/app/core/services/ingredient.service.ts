import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Ingredient } from '../models/ingredient';
import { PaginatedResult } from '../models/paginatedResult';

@Injectable({
  providedIn: 'root'
})
export class IngredientService {
  private baseUrl: string = environment.apiUrl;
  public paginatedResult: PaginatedResult<Ingredient[]> = new PaginatedResult<Ingredient[]>();

  constructor(private http: HttpClient) { }


  public addIngredient(ingredient: Ingredient, addAnyway: boolean): Observable<Object> {
    let params = new HttpParams();
    params = params.append('addAnyway', addAnyway.toString());

    return this.http.post(this.baseUrl + 'ingredient/add-ingredient', ingredient, {observe: 'response' , params});
  }

  public getIngredients(currentPage?: number, pageSize?: number): Observable<PaginatedResult<Ingredient[]>> {
    let params = new HttpParams();
    
    if(currentPage != null && pageSize != null) {
      params = params.append('currentPage', currentPage.toString());
      params = params.append('pageSize', pageSize.toString());
    }

    return this.http.get<Ingredient[]>(this.baseUrl + 'ingredient/get-ingredients',  {observe: 'response' , params})
    .pipe(
      map((response: HttpResponse<Ingredient[]>) => {
        this.paginatedResult.result = response.body;
        if(response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return this.paginatedResult;
      })
    )
  }
  
  public deleteIngredient(id: number): Observable<Object> {
    return this.http.delete(this.baseUrl + 'ingredient/delete-ingredient/' + id);
  }
}