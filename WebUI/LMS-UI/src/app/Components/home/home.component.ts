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
  successLabel!: boolean;
  notifyMessage: any;
  searchQuery = '';
  filteredCourses: any[] = [];
  isAdmin !: boolean;
  courses!: course[];
  currentPage = 1;
  itemsPerPage = 5;
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
        this.showError("An error occured. contact your system administrator.");
        this.toast.stopLoader();
      });
  }

  openAddCourseDialog() {
    const dialogRef = this.dialog.open(AddCoursePopupComponent, {
      width:'500px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getCourse();
        this.showSuccess("Course Added Successfully");
      }
      else {
        this.toast.stopLoader();
      }
    });
  }

  openDeleteCourseDialog(course: course) {
    var tech = course.technology;
    const dialogRef = this.dialog.open(DeleteCoursePopupComponent, {
      width: '300px',
      data: { tech }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deleteCourse(tech);
      }
      else {
        this.toast.stopLoader();
      }
    });
  }

  deleteCourse(courseName: string) {
    this.courseService.deleteCourse(courseName).subscribe(
      (response) => {
        this.showSuccess(response.message);
        this.getCourse();
      },
      (error) => {
        this.toast.stopLoader();
        this.showError("An error occured. contact your system administrator.");
      });
  }

  logout() {
    this.toast.showLoader();
    this.authService.logout();
    this.router.navigate(["login"]);
    this.toast.showSuccess('Logout Successfully!');
    this.toast.stopLoader();
  }

  //filterCourses() {
  //  this.filteredCourses = this.courses.filter((course) => {
  //    const searchQuery = this.searchQuery.toLowerCase();

  //    // Convert duration to a string for comparison
  //    const durationString = course.duration.toString().toLowerCase();

  //    return (
  //      course.name.toLowerCase().includes(searchQuery) ||
  //      course.description.toLowerCase().includes(searchQuery) ||
  //      course.technology.toLowerCase().includes(searchQuery) ||
  //      durationString.includes(searchQuery)
  //    );
  //  });
  //}

  closeNotify() {
    this.errorLabel = false;
    this.notifyMessage = null;
    this.successLabel = false;
  }

  showError(errMsg : string) {
    this.errorLabel = true;
    this.notifyMessage = errMsg;
    setTimeout(() => {
      this.closeNotify();
    }, 8000);
  }

  showSuccess(errMsg: string) {
    this.successLabel = true;
    this.notifyMessage = errMsg;
    setTimeout(() => {
      this.closeNotify();
    }, 8000);
  }

  get totalPages(): number {
    return Math.ceil(this.filteredCourses.length / this.itemsPerPage);
  }

  // Calculate the start and end index for the current page
  get startIndex(): number {
    return (this.currentPage - 1) * this.itemsPerPage;
  }

  get endIndex(): number {
    return this.currentPage * this.itemsPerPage;
  }

  // Function to navigate to the previous page
  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  // Function to navigate to the next page
  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }
}
