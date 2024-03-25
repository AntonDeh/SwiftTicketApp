import { __decorate } from "tslib";
import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
let AuthService = class AuthService {
    constructor(http, router) {
        this.http = http;
        this.router = router;
        this.loginUrl = 'https://localhost:7009/api/account/login';
    }
    get currentUser() {
        const token = localStorage.getItem('authToken');
        return token ? { token } : null;
    }
    getUserName() {
        // Example of getting username
        return localStorage.getItem('userName') || 'Unknown User';
    }
    // Method to check user authentication
    isAuthenticated() {
        const token = localStorage.getItem('authToken'); // the token is stored in localStorage
        return !!token; // Returns true if the token exists, false otherwise
    }
    // Method to save user token
    saveToken(token) {
        localStorage.setItem('authToken', token);
    }
    // Method to remove user token (exit)
    logout() {
        localStorage.removeItem('authToken');
    }
    login(model) {
        console.log('Login method called');
        return this.http.post(this.loginUrl, model, {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        }).pipe(tap(response => {
            // the server returns an object with a token in the `token` property
            console.log('Response from server:', response.token);
            this.saveToken(response.token); // Save the token
        }), catchError(error => {
            console.error('Login error', error);
            return throwError(() => error);
        }));
    }
};
AuthService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], AuthService);
export { AuthService };
//# sourceMappingURL=auth.service.js.map