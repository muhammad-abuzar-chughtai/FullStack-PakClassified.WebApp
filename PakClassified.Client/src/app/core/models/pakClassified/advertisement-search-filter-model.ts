export interface AdvertisementSearchFilter {
    cityAreaId?: number;
    postedById?: number;
    statusId?: number;
    typeId?: number;
    subCategoryId?: number;
    tagIds?: number[];
}

export interface SearchFilterModal extends AdvertisementSearchFilter {
    cityArea?: string;
    postedBy?: string;
    status?: string;
    type?: string;
    subCategory?: string;
    tagNames?: string[];
}