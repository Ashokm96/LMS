import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { course } from '../models/course';

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  constructor(private http: HttpClient, private router: Router) { }

  public getCourses(): Observable<course[]> {
    let url: string = environment.endpoints.apiBaseURL + environment.endpoints.getCourse;
    return this.http.get<course[]>(url);
  }

}
