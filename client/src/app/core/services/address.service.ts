import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Address } from '../models/address';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  private baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getAddresses(): Observable<Address[]> {
    return this.http.get<Address[]>(this.baseUrl + 'address/get-addresses');
  }

  public deleteAddress(id: number): Observable<Object> {
    return this.http.delete(this.baseUrl + 'address/delete-address/' + id);
  }

  public addAddress(address: Address): Observable<number> {
    return this.http.post<number>(this.baseUrl + 'address/add-address', address);
  }
}