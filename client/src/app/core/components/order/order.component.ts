import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Address } from '../../models/address';
import { Order } from '../../models/order';
import { OrderDishParameter } from '../../models/orderDishParameter';
import { AddressService } from '../../services/address.service';
import { DishCartService } from '../../services/dish-cart.service';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {
  public orderForm: FormGroup;
  public addresses: Address[] = [];
  public defaultTime: number[] = [new Date().getHours(), 0 , 0];

  constructor(private orderService: OrderService, private toastr: ToastrService, 
    public dishCartService: DishCartService, private addressService: AddressService,
    private formBuilder: FormBuilder, private datePipe: DatePipe, private router: Router) { }


  public ngOnInit(): void {
    this.loadAddresses();
    this.initializeForm();
  }

  public loadAddresses(): void {
    this.addressService.getAddresses().subscribe(((addresses: Address[]) => {
      this.addresses = addresses;
    }));
  }

  public resetDeliveryDate(): void {
    this.orderForm.controls['deliveryDate'].reset();
  }

  public getOrderSummary(): number {
    let sum: number = 0;

    this.dishCartService.dishCartItems.forEach(item => {
      sum += item.dishPrice * item.count * item.numberOfPersons;
    });

    return sum;
  }

  public makeOrder(): void {
    const orderDishParameters: OrderDishParameter[] = [];
    const deliveryDate = new Date(this.orderForm.controls['deliveryDate'].value);
    const orderDate = new Date();

    this.dishCartService.dishCartItems.forEach(item => {
      const orderDishParameter: OrderDishParameter = {
        dishId: item.dishId,
        numberOfPersons: item.numberOfPersons,
        count: item.count
      };
      orderDishParameters.push(orderDishParameter);
    });

    const order: Order = {
      addressId: (Number(this.orderForm.controls['addressId'].value) === 0) 
        ? this.addresses[0].id 
        : Number(this.orderForm.controls['addressId'].value),
      orderDate: this.datePipe.transform(orderDate, 'yyyy-MM-dd') + 'T' + this.datePipe.transform(orderDate, 'HH:mm'),
      deliveryDate: this.datePipe.transform(deliveryDate, 'yyyy-MM-dd') + 'T' + this.datePipe.transform(deliveryDate, 'HH:mm'),
      totalPrice: this.getOrderSummary(),
      status: 'accepted',
      orderDishParameters: orderDishParameters
    };

    this.orderService.makeOrder(order).subscribe(() => {
      this.router.navigateByUrl('');
      this.dishCartService.dishCartItems = [];
      this.toastr.success('Заказ успешно оформлен');
    });
  }

  private initializeForm(): void {
    this.orderForm = this.formBuilder.group({
      addressId: ['', []],
      deliveryDate: ['', [Validators.required]]
    });
  }
}