export interface OrderUserGet {
    id: number;
    orderDate: Date;
    deliveryDate: Date;
    totalPrice: number;
    evaluation?: number;
    status: string;
}