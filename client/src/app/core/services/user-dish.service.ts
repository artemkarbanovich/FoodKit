import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../models/paginatedResult';
import { UserDish } from '../models/userDish';

@Injectable({
  providedIn: 'root'
})
export class UserDishService {
  private baseUrl: string = environment.apiUrl;
  public paginatedResult: PaginatedResult<UserDish[]> = new PaginatedResult<UserDish[]>();
  
  constructor(private http: HttpClient) { }

  
  public addUserDish(userDish: UserDish): Observable<number> {
    return this.http.post<number>(this.baseUrl + 'userdish/add-user-dish', userDish);
  }

  public getUserDishes(currentPage?: number, pageSize?: number): Observable<PaginatedResult<UserDish[]>> {
    let params = new HttpParams();
    
    if(currentPage != null && pageSize != null) {
      params = params.append('currentPage', currentPage.toString());
      params = params.append('pageSize', pageSize.toString());
    }

    return this.http.get<UserDish[]>(this.baseUrl + 'userdish/get-user-dishes', {observe: 'response' , params})
    .pipe(
      map((response: HttpResponse<UserDish[]>) => {
        this.paginatedResult.result = response.body;
        if(response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return this.paginatedResult;
      })
    );
  }
}