import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Order } from '../models/order';
import { OrderUserGet } from '../models/orderUserGet';
import { PaginatedResult } from '../models/paginatedResult';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  public baseUrl: string = environment.apiUrl;
  public ordersUserPaginatedResult: PaginatedResult<OrderUserGet[]> = new PaginatedResult<OrderUserGet[]>();

  constructor(private http: HttpClient) { }
  
  public makeOrder(order: Order): Observable<Object> {
    return this.http.post(this.baseUrl + 'order/make-order', order);
  }

  public getUserOrders(currentPage?: number, pageSize?: number): Observable<PaginatedResult<OrderUserGet[]>> {
    let params = new HttpParams();
    
    if(currentPage != null && pageSize != null) {
      params = params.append('currentPage', currentPage.toString());
      params = params.append('pageSize', pageSize.toString());
    }

    return this.http.get<OrderUserGet[]>(this.baseUrl + 'order/get-user-orders', {observe: 'response', params})
    .pipe(
      map((response: HttpResponse<OrderUserGet[]>) => {
        this.ordersUserPaginatedResult.result = response.body;
        if(response.headers.get('Pagination') !== null) {
          this.ordersUserPaginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return this.ordersUserPaginatedResult;
      })
    );
  }

  public updateOrderStatus(id: number, status: string): Observable<Object> {
    return this.http.patch(this.baseUrl + 'order/update-order-status/' + id, { },
      { params: { status: status }});
  }

  public updateEvaluation(id: number, evaluationValue: number): Observable<Object> {
    return this.http.patch(this.baseUrl + 'order/update-evaluation/' + id, { },
      { params: { evaluationValue: evaluationValue }});
  }
}