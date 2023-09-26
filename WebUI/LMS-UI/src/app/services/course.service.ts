import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { course } from '../models/course';
import { user } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  constructor(private http: HttpClient, private router: Router) { }

  public getCourses(): Observable<course[]> {
    let url: string = environment.endpoints.apiBaseURL + environment.endpoints.getCourse;
    return this.http.get<course[]>(url);
  }

  public addCourse(course:course): Observable<any> {
    let url: string = environment.endpoints.apiBaseURL + environment.endpoints.addCourse;
    var returnValue = this.http.post<any>(url, course);
    return returnValue;
  }

  public deleteCourse(coursename: string): Observable<any> {
    let url: string = environment.endpoints.apiBaseURL + environment.endpoints.deleteCourse;
    url = url.replace('{coursename}',coursename.toString());
    var returnValue = this.http.delete<any>(url);
    return returnValue;
  }

  public registerUser(user: user): Observable<any> {
    let url: string = environment.endpoints.apiBaseURL + environment.endpoints.addUser;
    var returnValue = this.http.post<any>(url, user);
    return returnValue;
  }

}
