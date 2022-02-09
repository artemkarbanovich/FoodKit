export interface Address {
    id?: number;
    country: string;
    locality: string;
    street: string;
    houseNumber: string;
    apartmentNumber: string;
    entranceNumber: string | null;
    floor: string | null;
}