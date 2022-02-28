import { Image } from "./image";
import { Ingredient } from "./ingredient";

export interface Dish {
    id: number;
    name: string;
    cookingTimeHours: number;
    cookingTimeMinutes: number;
    youWillNeed: string;
    price: number;
    isAvailableForSingleOrder: boolean;
    images: Image[];
    ingredients: Ingredient[];
}