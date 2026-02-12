export const environment = {
  production: false,
  apiUrl: 'https://localhost:7053' // Backend API url
};

// api-endpoints.ts
export const API_ENDPOINTS = {
  Auth: 'api/Auth',

  Country: 'api/Country',
  Province: 'api/Province',
  City: 'api/City',
  CityArea: 'api/CityArea',

  Advertisement: 'api/Advertisement',
  AdvertisementType: 'api/AdvertisementType',
  AdvertisementTag: 'api/AdvertisementTag',
  AdvertisementSubCategory: 'api/AdvertisementSubCategory',
  AdvertisementCategory: 'api/AdvertisementCategory',
  AdvertisementStatus: 'api/AdvertisementStatus',
  AdvertisementImage: 'api/AdvertisementImage',

  User: 'api/User',
  Role: 'api/Role',
};
