export interface AdvertisementImagePost {
    id: number;
    name: string;
    content: File;
    caption?: string;
    createdBy: string;
    lastModifiedBy?: string;
    advertisementId: number;
}

export interface AdvertisementImageGet {
    id: number;
    name: string;
    content: string;
    caption?: string;
    createdBy: string;
    lastModifiedBy?: string;
    advertisementId: number;
}