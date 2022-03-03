import { OrderDishParameter } from "./orderDishParameter";

export interface Order {
    id?: number;
    addressId: number;
    orderDate?: string;
    deliveryDate: string;
    totalPrice: number;
    evaluation?: number;
    orderDishParameters: OrderDishParameter[];
}