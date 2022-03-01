export interface Ingredient {
    id?: number;
    name: string;
    proteins: number;
    fats: number;
    carbohydrates: number;
    calories: number;
    ingredientWeightPerPortion?: number | null;
}