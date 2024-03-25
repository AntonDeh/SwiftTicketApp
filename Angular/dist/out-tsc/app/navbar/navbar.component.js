import { __decorate } from "tslib";
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
let NavbarComponent = class NavbarComponent {
    constructor(authService, router) {
        this.authService = authService;
        this.router = router;
        this.model = { email: '', password: '', rememberMe: false };
        this.loggedIn = false;
    }
    ngOnInit() {
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
NavbarComponent = __decorate([
    Component({
        selector: 'app-navbar',
        standalone: true,
        imports: [
            CommonModule,
            ReactiveFormsModule,
            FormsModule,
        ],
        templateUrl: './navbar.component.html',
        styleUrls: ['./navbar.component.scss']
    })
], NavbarComponent);
export { NavbarComponent };
//# sourceMappingURL=navbar.component.js.map