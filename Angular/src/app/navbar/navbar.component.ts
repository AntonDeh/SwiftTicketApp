import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; 
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit {
  model: any = { email: '', password: '', rememberMe: false };
  loggedIn: boolean = false;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }

  login() {
    this.authService.login(this.model).subscribe({
      next: response => {
        console.log('Login successful', response);
        this.loggedIn = true;

      },
      error: (error) => {
        console.error('Login error', error);
      }
      })
  }
}
