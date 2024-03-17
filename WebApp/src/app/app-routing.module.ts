import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationComponent } from './components/authentication/authentication.component';
import { RegistrationComponent } from './components/authentication/registration/registration.component';
import { HomeComponent } from './components/home/home.component';
import { AuthGuardGuard } from './guards/auth-guard.guard';
import { SetAdminComponent } from './components/authentication/set-admin/set-admin.component';
import { SensorDataComponent } from './components/sensor-data/sensor-data.component';
import { WebScrapingComponent } from './components/web-scraping/web-scraping.component';


const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'prefix' },
  { path: 'home', component: HomeComponent},
  { path: 'login', component: AuthenticationComponent},
  { path: 'register', component: RegistrationComponent},
  { path: 'users', component: SetAdminComponent, canActivate:[AuthGuardGuard]},
  { path: 'sensor-data', component: SensorDataComponent, canActivate:[AuthGuardGuard]},
  { path: 'web-scraping', component: WebScrapingComponent, canActivate:[AuthGuardGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
