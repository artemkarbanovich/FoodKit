import { OrderDishParameter } from "./orderDishParameter";

export interface Order {
    addressId: number;
    orderDate: string;
    deliveryDate: string;
    totalPrice: number;
    status: string;
    orderDishParameters: OrderDishParameter[];
}