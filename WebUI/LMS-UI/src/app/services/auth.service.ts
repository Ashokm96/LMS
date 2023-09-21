import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { login } from '../models/login';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private router: Router) { }

  public login(loginCredntials:login) :Observable<any> {
    let url: string = environment.endpoints.apiBaseURL + environment.endpoints.login;
    return this.http.post(url,loginCredntials);
  }

  isLoggedIn(): boolean {
    //check if user is logged in
    const token = localStorage.getItem('token');
    return !!token;
  }

  logout() :void {
    localStorage.removeItem('token');
  }

  hasAnyRole(role:String[]):boolean {
    const token = localStorage.getItem('token');
    if (token) {
      const decodedToken = this.decodeToken(token);
      const userRole = decodedToken.role;
      return role.includes(userRole);
    }
    return false;
  }

  isAdmin(): boolean {
    // Check if the user has the 'Admin' role
    const token = localStorage.getItem('token');
    if (token) {
      const decodedToken = this.decodeToken(token);
      const userRole = decodedToken.role;
      return userRole === 'Admin';
    }
    return false;
  }
  private decodeToken(token: string): any {
    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch (error) {
      return null;
    }
  }

}
