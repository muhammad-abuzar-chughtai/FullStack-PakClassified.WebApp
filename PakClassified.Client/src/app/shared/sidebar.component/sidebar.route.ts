import { Routes } from '@angular/router';
import { CountryComponent } from '../../components/location/country.component/country.component';
import { ProvinceComponent } from '../../components/location/province.component/province.component';
import { CityComponent } from '../../components/location/city.component/city.component';
import { CityAreaComponent } from '../../components/location/cityarea.component/cityarea.component';
import { AdminLayout } from '../../layouts/admin-layout/admin-layout';
import { WelcomeAdmin } from '../../layouts/admin-layout/welcome-admin/welcome-admin';

export const AdminRoutes: Routes = [
  {
    path: 'admin',
    component: AdminLayout,
    children: [
      { path: 'countries', component: CountryComponent },
      { path: 'provinces', component: ProvinceComponent },
      { path: 'cities', component: CityComponent },
      { path: 'cityareas', component: CityAreaComponent },
      { path: '', component: WelcomeAdmin }, // default dashboard
    ]
  }
];



