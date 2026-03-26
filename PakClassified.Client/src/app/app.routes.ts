import { Routes } from '@angular/router';
import { AdminLayout } from './layouts/admin-layout/admin-layout';
import { CountryComponent } from './components/location/country.component/country.component';
import { ProvinceComponent } from './components/location/province.component/province.component';
import { CityComponent } from './components/location/city.component/city.component';
import { CityAreaComponent } from './components/location/cityarea.component/cityarea.component';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';
import { WelcomeAdmin } from './layouts/admin-layout/welcome-admin/welcome-admin';
import { AdControl } from './components/PakClassified/ad-control/ad-control';
import { AdvertisementStatusComponent } from './components/PakClassified/ad-control/advertisement-status/advertisement-status';
import { AdvertisementTypeComponent } from './components/PakClassified/ad-control/advertisement-type/advertisement-type';
import { AdvertisementTagsComponent } from './components/PakClassified/ad-control/advertisement-tags/advertisement-tags';
import { AdTaxonomy } from './components/PakClassified/ad-taxonomy/ad-taxonomy';
import { AdvertisementCategoryComponent } from './components/PakClassified/ad-taxonomy/advertisement-category/advertisement-category';
import { AdvertisementSubcategoryComponent } from './components/PakClassified/ad-taxonomy/advertisement-subcategory/advertisement-subcategory';
import { AdvertisementComponent } from './components/PakClassified/advertisement/advertisement';
import { AdvertisementImageComponent } from './components/PakClassified/advertisement-image/advertisement-image';
import { AdPage } from './components/PakClassified/advertisement/ad-page/ad-page';
import { CreateEdit } from './components/PakClassified/advertisement/create-edit/create-edit';
import { UserSettings } from './components/user-settings/user-settings';
import { UserComponent } from './components/user-settings/user.component/user.component';
import { RoleComponent } from './components/user-settings/role.component/role.component';
import { guestGuard } from './core/guards/guest.guard';
import { DefaultLayout } from './layouts/default-layout/default-layout';
import { AboutSectionComponent } from './components/Web-Components/about-section.component/about-section.component';
import { AdvertisementPageLayout } from './layouts/advertisement-page-layout/advertisement-page-layout';

export const routes: Routes = [
  {
    path: 'admin',
    canActivate: [authGuard, roleGuard],
    data: { roleId: [4] },
    component: AdminLayout,
    children: [
      { path: 'countries', component: CountryComponent },
      { path: 'provinces', component: ProvinceComponent },
      { path: 'cities', component: CityComponent },
      { path: 'cityareas', component: CityAreaComponent },

      { path: 'advertisements', component: AdvertisementComponent },
      { path: 'advertisement/create', component: CreateEdit },
      { path: 'advertisement/edit/:id', component: CreateEdit },
      { path: 'ad-images', component: AdvertisementImageComponent },

      {
        path: 'ad-control',
        component: AdControl,
        children: [
          { path: 'status', component: AdvertisementStatusComponent },
          { path: 'type', component: AdvertisementTypeComponent },
          { path: 'tags', component: AdvertisementTagsComponent },
          { path: '', redirectTo: 'status', pathMatch: 'full' } // default to status
        ]
      },
      {
        path: 'ad-taxonomy',
        component: AdTaxonomy,
        children: [
          { path: 'category', component: AdvertisementCategoryComponent },
          { path: 'subcategory', component: AdvertisementSubcategoryComponent },
          { path: '', redirectTo: 'category', pathMatch: 'full' } // default to Category
        ]
      },

      {
        path: 'user-settings',
        component: UserSettings,
        children: [
          { path: 'users', component: UserComponent },
          { path: 'roles', component: RoleComponent },
          { path: '', redirectTo: 'users', pathMatch: 'full' } // default to users
        ]
      },

      { path: '', component: WelcomeAdmin }, // default dashboard
    ]
  },

  { path: 'advertisement/:id', component: AdPage },


  { path: '', component: DefaultLayout, },

  { path: 'advertisement-page', component: AdvertisementPageLayout, },
  // { path: '**', redirectTo: '', pathMatch: 'full' },  // Wild Card




  {
    path: 'login',
    canActivate: [guestGuard],   // ← add this
    loadComponent: () =>
      import('./layouts/auth-layout/auth-layout')
        .then(m => m.AuthComponent)
  },
];
