import { UserGet } from "../user/user-model";

export interface AdvertisementGetPost {
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
    cityAreaId: number;
    postedById: number;
    statusId: number;
    typeId: number;
    subCategoryId: number;
    tagsId: number[];
    imagesId: number[];
}
export interface Advertisement extends AdvertisementGetPost {
    cityArea?: string;
    postedBy?: UserGet;
    status?: string;
    type?: string;
    subCategory?: string;
    tagNames?: string[];
}