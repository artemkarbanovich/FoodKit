import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Register } from '../models/register';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl: string = environment.apiUrl;


  constructor(private http: HttpClient) { }


  public register(user: Register) {
    return this.http.post(this.baseUrl + 'account/register', user);
  }

  private setCurrentUser() {

  }
}