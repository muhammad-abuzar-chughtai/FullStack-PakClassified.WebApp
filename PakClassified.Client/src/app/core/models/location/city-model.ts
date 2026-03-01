export interface City {
    id: number;
    name: string;
    createdBy: string;
    lastModifiedBy?: string;
    provinceId: number;
    provinceName?: string; // Optional for display purposes
}