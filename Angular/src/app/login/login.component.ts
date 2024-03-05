import { Component } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  model: any = {
    email: '',
    password: '',
    rememberMe: false
  };



  constructor(private authService: AuthService) { }

/*  testLogin() {
    this.authService.login({ username: 'test', password: 'password' }).subscribe({
      next: (result) => {
        console.log('Login success', result);
      },
      error: (error) => {
        console.error('Login failed', error);
      }
    });
  }
  */
}
