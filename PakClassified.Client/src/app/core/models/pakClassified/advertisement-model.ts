import { AdvertisementTag } from "./advertisement-tag-model";

export interface Advertisement {
    id: number;
    name: string;
    title: string;
    description?: string;
    price: number;
    likes: number;
    startsOn: Date;
    endsOn: Date;
    createdBy: string;
    lastModifiedBy?: string;
    citiyAreaId: number;
    postedById: number;
    statusId: number;
    typeId: number;
    subCategoryId: number;
    tags: AdvertisementTag[];
    
}