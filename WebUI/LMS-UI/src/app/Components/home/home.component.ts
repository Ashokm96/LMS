import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastService } from '../../Common/toast.service';
import { course } from '../../models/course';
import { AuthService } from '../../services/auth.service';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  //courses = [
  //  {
  //    name: 'fgf',
  //    description: 'Description 1',
  //    technology: 'Technology 1',
  //    duration: 60,
  //    launchUrl: 'https://example.com/course1'
  //  },
  //  {
  //    name: 'ww',
  //    description: 'Description 2',
  //    technology: 'Technology 2',
  //    duration: 50,
  //    launchUrl: 'https://example.com/course2'
  //  },
  //  {
  //    name: 'rr',
  //    description: 'Description 2',
  //    technology: 'Technology 2',
  //    duration: 150,
  //    launchUrl: 'https://example.com/course2'
  //  }
  //  // Add more courses as needed
  //];

  searchQuery = '';
  filteredCourses: any[] = [];
  isAdmin !: boolean;
  courses!: course[];
  constructor(private authService: AuthService, private router: Router, private toast: ToastService,private courseService:CourseService) { }

  ngOnInit(): void {
    // Initialize filteredCourses with all courses
    this.getCourse();
    this.isAdmin = this.authService.isAdmin();
   
  }

  getCourse() {
    this.toast.showLoader();
    var res = this.courseService.getCourses().subscribe(
      (response) => {
        this.courses = response;
        this.filteredCourses = [...this.courses];
        this.toast.stopLoader();
      },
      (error) => {
        this.toast.stopLoader();
        this.toast.showError(error.error.message);
      });
  }

  addCourse() {
    // Implement the logic to add a new course
    // You can open a dialog or navigate to a new page for adding a course
  }

  deleteCourse(course: any) {
    // Implement the logic to delete the selected course
    // You may want to ask for confirmation before deleting
  }

  logout() {
    this.toast.showLoader();
    this.authService.logout();
    this.router.navigate(["login"]);
    this.toast.showSuccess('Logout Successfully');
    this.toast.stopLoader();
  }

  // Implement a filter function for search
  //filterCourses() {
  //  this.filteredCourses = this.courses.filter((course) => {
  //    return (
  //      course.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
  //      course.description.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
  //      course.technology.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
  //      course.duration.toLowerCase().includes(this.searchQuery.toLowerCase())
  //    );
  //  });
  //}
  filterCourses() {
    this.filteredCourses = this.courses.filter((course) => {
      const searchQuery = this.searchQuery.toLowerCase();

      // Convert duration to a string for comparison
      const durationString = course.Duration.toString().toLowerCase();

      return (
        course.Name.toLowerCase().includes(searchQuery) ||
        course.Description.toLowerCase().includes(searchQuery) ||
        course.Technology.toLowerCase().includes(searchQuery) ||
        durationString.includes(searchQuery)
      );
    });
  }

}
