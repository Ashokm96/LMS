import { Component, EventEmitter, Output, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastService } from '../../Common/toast.service';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-delete-course-popup',
  templateUrl: './delete-course-popup.component.html',
  styleUrls: ['./delete-course-popup.component.scss']
})
export class DeleteCoursePopupComponent {
  @Output() confirmed = new EventEmitter<boolean>();
  courseName: any;
  constructor(public dialogRef: MatDialogRef<DeleteCoursePopupComponent>, private dialog: MatDialog, @Inject(MAT_DIALOG_DATA) public data: any,
    public courseService: CourseService,public toast: ToastService) { }

  ngOnInit() {
    this.courseName = this.data.tech;
  }

  confirmDelete() {
    this.toast.showLoader();
    this.dialogRef.close(true);
    //this.deleteCourse(this.courseName)
    //  .then(() => this.dialogRef.close(true))
    //  .catch(error=>console.log(error));
    //this.confirmed.emit(true);
  }

  deleteCourse(courseName:string) : Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.courseService.deleteCourse(courseName).subscribe({
        next: response => {
          console.log(response);
        },
        error: err => {
          console.log(err);
        }
      });
    });
  }

  cancelDelete() {
    this.confirmed.emit(false);
  }
}
