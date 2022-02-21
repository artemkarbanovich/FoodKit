import { DishAddIngredient } from "./dishAddIngredient";

export interface DishAdd {
    name: string;
    cookingTimeHours: number;
    cookingTimeMinutes: number;
    youWillNeed: string;
    price: number;
    isAvailableForSingleOrder: boolean;
    ingredients: DishAddIngredient[];
    images: File[];
}