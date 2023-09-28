  import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
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
    isAdmin !: boolean;
    courses!: course[];
    currentPage = 1;
    itemsPerPage = 8;
    filterForm!: FormGroup;
    constructor(private authService: AuthService, private router: Router, private toast: ToastService, private courseService: CourseService, private dialog: MatDialog, private formBuilder: FormBuilder) { }

    ngOnInit(): void {
      this.isAdmin = this.authService.isAdmin();
      this.getCourse();
      this.initilizeForm();
    }

    initilizeForm() {
      this.filterForm = this.formBuilder.group({
        technology: ['', Validators.required],
        durationFrom: [null],
        durationTo: [null],
      }, {
        validator: this.durationValidation
      });
    }

    durationValidation(group: AbstractControl): { [key: string]: boolean } | null {
      const durationFrom = group.get('durationFrom')?.value;
      const durationTo = group.get('durationTo')?.value;
      if (durationFrom !== null && durationTo === null) {
        return { 'durationToRequired': true };
      }
      if (durationFrom !== null && durationTo !== null && durationFrom >= durationTo) {
        return { 'durationToRequired': true };
      }
      return null;
    }

    applyFilter() {
      this.toast.showLoader();
      if (this.filterForm.valid) {
        const durationFrom = this.filterForm.get('durationFrom')?.value;
        const durationTo = this.filterForm.get('durationTo')?.value;
        const technology = this.filterForm.get('technology')?.value;
        if (durationFrom!=null && durationTo!=null && technology!=null) {
          this.getCourseByDuration(technology,durationFrom,durationTo);
        } else if (technology != null) {
          this.getCourseByTech(technology);
        } 
      } else {
        console.log('Please fill out all fields with valid values.');
      }
    }

    getCourseByTech(technology:string) {
      this.courseService.getCoursesByTech(technology).subscribe({
        next: response => {
          this.courses = response;
          this.toast.stopLoader();
        },
        error: err => {
          this.showError("An error occured. contact your system administrator.");
          this.toast.stopLoader();
        }
      });
    }

    getCourseByDuration(technology: string, durationFrom: number, durationTo:number) {
      this.courseService.getCoursesByDuration(technology,durationFrom,durationTo).subscribe({
        next: response => {
          this.courses = response;
          this.toast.stopLoader();
        },
        error: err => {
          this.showError("An error occured. contact your system administrator.");
          this.toast.stopLoader();
        }
      });
    }


    getCourse() {
      this.toast.showLoader();
      var res = this.courseService.getCourses().subscribe(
        (response) => {
          this.courses = response;
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
          this.refreshPagesAfterAdded();
          this.showSuccess("Course Added Successfully");
        }
        else {
          this.toast.stopLoader();
        }
      });
    }

    refreshPagesAfterAdded() {
      this.courseService.getCourses().subscribe(
        (response) => {
          this.courses = response;
          var count = this.totalPages;
          if (count == 1) {
            this.currentPage = 1;
          } else {
            this.currentPage = count;
          }
          this.toast.stopLoader();
        },
        (error) => {
          this.showError("An error occured. contact your system administrator.");
          this.toast.stopLoader();
        });
   
    }

    openDeleteCourseDialog(course: course) {
      var tech = course.name;
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
          this.refreshPagesAfterDeleted(courseName);
          this.getCourse();
        },
        (error) => {
          this.toast.stopLoader();
          this.showError("An error occured. contact your system administrator.");
        });
    }

    refreshPagesAfterDeleted(courseName:string) {
      // Remove the deleted course from the courses array
      this.courses = this.courses.filter(course => course.technology !== courseName);
      // Reset the current page to the first page
      var count = this.totalPages;
      if (count == 1) {
        this.currentPage = 1;
      }
    }

    logout() {
      this.toast.showLoader(); 
      this.authService.logout();
      this.router.navigate(["login"]);
      //this.toast.showSuccess('Logout Successfully!');
      this.toast.stopLoader();
    }
   
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
      return Math.ceil(this.courses.length / this.itemsPerPage);
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
