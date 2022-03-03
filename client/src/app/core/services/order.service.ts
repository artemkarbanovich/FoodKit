import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Order } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  public baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }
  
  public makeOrder(order: Order): Observable<Object> {
    return this.http.post(this.baseUrl + 'order/make-order', order);
  }
}