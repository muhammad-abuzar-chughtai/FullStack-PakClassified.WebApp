import { provideRouter, Routes } from '@angular/router';
import { AdminLayout } from './layouts/admin-layout/admin-layout';
import { CountryComponent } from './components/location/country.component/country.component';
import { ProvinceComponent } from './components/location/province.component/province.component';
import { CityComponent } from './components/location/city.component/city.component';
import { CityAreaComponent } from './components/location/cityarea.component/cityarea.component';
import { authGuard } from './core/guards/auth.guard/auth.guard';
import { roleGuard } from './core/guards/role.guard/role.guard';
import { WelcomeAdmin } from './layouts/admin-layout/welcome-admin/welcome-admin';

export const routes: Routes = [
  {
    path: 'admin',
    component: AdminLayout,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] },
    children: [
      { path: 'countries', component: CountryComponent },
      { path: 'provinces', component: ProvinceComponent },
      { path: 'cities', component: CityComponent },
      { path: 'cityareas', component: CityAreaComponent },
      { path: '', component: WelcomeAdmin }, // default dashboard
    ]
  },

  { path: '', redirectTo: 'login', pathMatch: 'full' },

  {
    path: 'login',
    loadComponent: () =>
      import('./layouts/auth-layout/auth-layout')
      .then(m => m.AuthComponent)
  },
];
