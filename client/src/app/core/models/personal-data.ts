export interface PersonalData {
    userName: string;
    name: string;
    phoneNumber: string;
    email: string;
    registrationDate?: Date;
    dateOfBirth: Date | string | null;
    gender: number | null;
    bodyWeight: number | null;
    bodyHeight: number | null;
    physicalActivityCoefficient: number | null;
}