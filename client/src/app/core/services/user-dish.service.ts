import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserDish } from '../models/userDish';

@Injectable({
  providedIn: 'root'
})
export class UserDishService {
  private baseUrl: string = environment.apiUrl;
  
  constructor(private http: HttpClient) { }

  public addUserDish(userDish: UserDish): Observable<Object> {
    return this.http.post(this.baseUrl + 'userdish/add-user-dish', userDish);
  }
}