export interface UserDish {
    id?: number;
    name: string;
    dishDate: Date | string;
    dishWeight: number;
    proteins: number;
    fats: number;
    carbohydrates: number;
    calories: number;
}