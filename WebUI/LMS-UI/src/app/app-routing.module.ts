import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { HomeComponent } from './Components/home/home.component';
import { AuthGuard } from './Common/auth.guard';
import { RegistrationComponent } from './Components/registration/registration.component';

const routes: Routes = [
  {path:'login',component:LoginComponent},
  { path: ' ', redirectTo: '/login', pathMatch: 'full' },
  //{ path: 'home', component: HomeComponent, canActivate: [AuthGuard], data: {'role':'Admin'} },
  { path: 'home', component: HomeComponent },
  { path: 'register', component: RegistrationComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
