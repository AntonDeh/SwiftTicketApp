import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './navbar/navbar.component';

import { CreateTicketComponent } from './create-ticket/create-ticket.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'create-ticket', component: CreateTicketComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' }
];
