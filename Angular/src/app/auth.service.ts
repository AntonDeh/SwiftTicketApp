import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loginUrl = 'https://localhost:7009/api/account/login';
  constructor(private http: HttpClient) { }


  login(model: any): Observable<any> {
    return this.http.post(this.loginUrl, model, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    }).pipe(
      catchError(error => {
        console.error('Login error', error);
        return throwError(() => error);
      })
    );
  }


}
