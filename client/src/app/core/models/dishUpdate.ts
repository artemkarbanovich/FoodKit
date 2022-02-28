export interface DishUpdate {
    id: number;
    name: string;
    cookingTimeHours: number;
    cookingTimeMinutes: number;
    youWillNeed: string;
    price: number;
    isAvailableForSingleOrder: boolean;
}