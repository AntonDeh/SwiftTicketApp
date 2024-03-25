import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
let LoginComponent = class LoginComponent {
    constructor(authService, router) {
        this.authService = authService;
        this.router = router;
        this.model = { email: '', password: '', rememberMe: false };
        this.loggedIn = false;
    }
    login() {
        this.authService.login(this.model).subscribe({
            next: response => {
                console.log('Login successful', response);
                this.loggedIn = true;
                this.router.navigate(['/dashboard']);
            },
            error: (error) => {
                console.error('Login error', error);
            }
        });
    }
};
LoginComponent = __decorate([
    Component({
        selector: 'app-login',
        templateUrl: './login.component.html',
        styleUrls: ['./login.component.scss'],
        standalone: true,
        imports: [
            CommonModule,
            ReactiveFormsModule,
            FormsModule,
        ],
    })
], LoginComponent);
export { LoginComponent };
//# sourceMappingURL=login.component.js.map