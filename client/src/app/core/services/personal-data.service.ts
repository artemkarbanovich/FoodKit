import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PersonalData } from '../models/personal-data';

@Injectable({
  providedIn: 'root'
})
export class PersonalDataService {
  private baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getPersonalData(userName: string): Observable<PersonalData> {
    return this.http.get<PersonalData>(this.baseUrl + 'user/get-personal-data/' + userName);
  }

  public updatePersonalData(user: PersonalData): Observable<Object> {
    return this.http.put(this.baseUrl + 'user/update-personal-data', user);
  }
}