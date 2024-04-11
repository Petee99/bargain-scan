import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationComponent } from './components/authentication/authentication.component';
import { RegistrationComponent } from './components/authentication/registration/registration.component';
import { HomeComponent } from './components/home/home.component';
import { AuthGuardGuard } from './guards/auth-guard.guard';
import { SetAdminComponent } from './components/authentication/set-admin/set-admin.component';
import { RetailerDataComponent } from './components/retailer-data/retailer-data.component';
import { WebScrapingComponent } from './components/web-scraping/web-scraping.component';


const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'prefix' },
  { path: 'home', component: HomeComponent},
  { path: 'login', component: AuthenticationComponent},
  { path: 'register', component: RegistrationComponent},
  { path: 'users', component: SetAdminComponent, canActivate:[AuthGuardGuard]},
  { path: 'retailer-data', component: RetailerDataComponent, canActivate:[AuthGuardGuard]},
  { path: 'web-scraping', component: WebScrapingComponent, canActivate:[AuthGuardGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
