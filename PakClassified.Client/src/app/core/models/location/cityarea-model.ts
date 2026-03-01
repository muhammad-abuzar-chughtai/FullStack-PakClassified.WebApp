export interface CityArea {
    id: number;
    name: string;
    createdBy: string;
    lastModifiedBy?: string;
    cityId: number;
    cityName?: string; // For display purposes
}