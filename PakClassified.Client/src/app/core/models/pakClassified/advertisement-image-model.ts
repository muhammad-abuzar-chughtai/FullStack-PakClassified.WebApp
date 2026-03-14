export interface AdvertisementImagePost {
    id: number;
    name: string;
    contentFile: File;
    caption?: string;
    createdBy: string;
    lastModifiedBy?: string;
    advertisementId: number;
    advertisement?: string;
}

export interface AdvertisementImageGet {
    id: number;
    name: string;
    content: string;
    caption?: string;
    createdBy: string;
    lastModifiedBy?: string;
    advertisementId: number;
    advertisement?: string;
}