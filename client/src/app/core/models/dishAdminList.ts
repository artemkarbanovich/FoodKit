export interface DishAdminList { 
    id: number;
    name: string;
    price: number;
    isAvailableForSingleOrder: boolean;
    imagePath: string | null;
}