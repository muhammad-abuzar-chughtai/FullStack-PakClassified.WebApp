export interface Province {
    id: number;
    name: string;
    createdBy: string;
    lastModifiedBy?: string;
    countryId: number;
    countryName?: string; // Optional for display purposes
}