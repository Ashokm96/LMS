import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastService } from '../../Common/toast.service';
import { AddCoursePopupComponent } from '../../dialogues/add-course-popup/add-course-popup.component';
import { DeleteCoursePopupComponent } from '../../dialogues/delete-course-popup/delete-course-popup.component';
import { course } from '../../models/course';
import { AuthService } from '../../services/auth.service';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  errorLabel!: boolean;
  errorLabelMessage: any;
  searchQuery = '';
  filteredCourses: any[] = [];
  isAdmin !: boolean;
  courses!: course[];
  constructor(private authService: AuthService, private router: Router, private toast: ToastService,private courseService:CourseService,private dialog:MatDialog) { }

  ngOnInit(): void {
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
        this.errorLabel = true;
        this.errorLabelMessage = error.message;
        this.toast.stopLoader();
        this.toast.showError(error.error.message);
        console.log(error.message);
      });
  }

  openAddCourseDialog() {
    const dialogRef = this.dialog.open(AddCoursePopupComponent, {
      width:'500px'
    });

    dialogRef.afterClosed().subscribe();
  }

  openDeleteCourseDialog(course:course) {
    const dialogRef = this.dialog.open(DeleteCoursePopupComponent, {
      width: '300px',
      data: { course }
    });

    dialogRef.afterClosed().subscribe();
  }

  logout() {
    this.toast.showLoader();
    this.authService.logout();
    this.router.navigate(["login"]);
    this.toast.showSuccess('Logout Successfully!');
    this.toast.stopLoader();
  }

  filterCourses() {
    this.filteredCourses = this.courses.filter((course) => {
      const searchQuery = this.searchQuery.toLowerCase();

      // Convert duration to a string for comparison
      const durationString = course.duration.toString().toLowerCase();

      return (
        course.name.toLowerCase().includes(searchQuery) ||
        course.description.toLowerCase().includes(searchQuery) ||
        course.technology.toLowerCase().includes(searchQuery) ||
        durationString.includes(searchQuery)
      );
    });
  }

}
