import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { CancellationConfirmationComponent } from 'src/app/shared/components/cancellation-confirmation/cancellation-confirmation.component';
import { OrderUserGet } from '../../models/orderUserGet';
import { PaginatedResult } from '../../models/paginatedResult';
import { Pagination } from '../../models/pagination';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-user-orders',
  templateUrl: './user-orders.component.html',
  styleUrls: ['./user-orders.component.scss']
})
export class UserOrdersComponent implements OnInit {
  public pagination: Pagination;
  public pageEvent: PageEvent;
  public orders: OrderUserGet[] = [];
  public displayColumns: string[] = 
    ['id', 'orderDate', 'deliveryDate', 'totalPrice', 'status', 'evaluation', 'cancelOrder'];
  public secondEvaluationClickTrigger = false;
  
  constructor(private orderService: OrderService, private toastr: ToastrService,
    private dialog: MatDialog) { }


  public ngOnInit(): void {
    this.loadOrders();
  }

  public handlePage(event?: PageEvent): PageEvent {
    this.pagination.currentPage = event.pageIndex;
    this.pagination.pageSize = event.pageSize;
    this.loadOrders();
    return event;
  }

  public loadOrders(): void {
    this.orderService.getUserOrders(this.pagination?.currentPage + 1, this.pagination?.pageSize)
    .subscribe((paginatedResult: PaginatedResult<OrderUserGet[]>) => {
      this.orders = paginatedResult.result;
      this.pagination = paginatedResult.pagination;
      this.pagination.currentPage = this.pagination.currentPage - 1;
    });
  }

  public updateEvaluation(orderId: number, evaluationValue: number): void {
    if(!this.secondEvaluationClickTrigger) {
      this.secondEvaluationClickTrigger = true;
      return;
    }
    
    this.orderService.updateEvaluation(orderId, evaluationValue).subscribe(() => { });
    this.secondEvaluationClickTrigger = false;
  }

  public cancelOrder(orderId: number): void {
    this.dialog.open(CancellationConfirmationComponent, {disableClose: true, autoFocus: false})
    .afterClosed().subscribe(result => {
      if(result == false) return;

      this.orderService.updateOrderStatus(orderId, 'canceled').subscribe(() => {
        this.loadOrders();
        this.toastr.success('Заказ успешно отменен');
      });
    });
  }

  public statusTranslator(status: string): string {
    switch(status) {
      case 'accepted': return 'Принят';
      case 'in way': return 'В пути';
      case 'canceled': return 'Отменен';
      case 'delivered': return 'Доставлен';
      default: return '-';
    }
  }
}