import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { NavbarComponent } from './navbar/navbar.component';

// Import the created components
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { CreateTicketComponent } from './create-ticket/create-ticket.component';
// Import routes
import { routes } from './app.routes';
import { AuthService } from './auth.service';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    CreateTicketComponent,
    NavbarComponent,
    CommonModule 
    // Declare all components used in the application
  ],
  imports: [
    HttpClientModule,
    FormsModule,
    BrowserModule,
    RouterModule.forRoot(routes),
    ReactiveFormsModule,
    // Import other required modules
  ],
  providers: [AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
