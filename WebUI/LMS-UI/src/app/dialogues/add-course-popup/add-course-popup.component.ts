import { Component, EventEmitter, Inject, Input, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastService } from '../../Common/toast.service';
import { course } from '../../models/course';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-add-course-popup',
  templateUrl: './add-course-popup.component.html',
  styleUrls: ['./add-course-popup.component.scss']
})
export class AddCoursePopupComponent {
  courseForm!: FormGroup;
  course: course = {} as course; 
  existingCourseName!: boolean;
  constructor(public dialogRef: MatDialogRef<AddCoursePopupComponent>, private dialog: MatDialog, private fb: FormBuilder,
    private courseService: CourseService, private toastService: ToastService, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.courseForm = this.fb.group({
      name: ['', [Validators.required, this.nonEmptyValidator, Validators.minLength(5)]],
      description: ['', [Validators.required, this.nonEmptyValidator, Validators.minLength(20)]],
      technology: ['', Validators.required],
      duration: ['', [Validators.required, Validators.pattern(/^\d+$/), Validators.min(0)]],
      launchUrl: ['', [Validators.required, Validators.pattern('https?://.+')]] // Use a regex pattern for URL validation
    });
  }

  nonEmptyValidator(control: AbstractControl): { [key: string]: boolean } | null {
    if (control.value === null || control.value.trim() === '') {
      return { 'nonEmpty': true };
    }
    return null;
  }

  emptyError(){
    this.existingCourseName = false;
  }

  onSave() {
    this.existingCourseName = false;
      this.course.name = this.courseForm.get('name')?.value;
      this.course.technology = this.courseForm.get('technology')?.value;
      this.course.description = this.courseForm.get('description')?.value;
      this.course.duration = this.courseForm.get('duration')?.value;
      this.course.launchUrl = this.courseForm.get('launchUrl')?.value;
    this.toastService.showLoader();

    this.courseService.getCourses().subscribe(
      (existingCourses: course[]) => {
        // Check if the new course name is a duplicate
        if (this.isDuplicateName(existingCourses, this.course.name)) {
          this.toastService.stopLoader();
          // Display an error message (e.g., using toastService)
          //this.toastService.showError('Course name already exists.');
          this.existingCourseName = true;
        } else {
          // If not a duplicate, add the course
          this.addCourse(this.course)
            .then(() => this.dialogRef.close(true))
            .catch(error => console.log(error));
        }
      },
      error => {
        this.toastService.stopLoader();
        console.log(error);
      }
    );
  }


  isDuplicateName(existingCourses: course[], newName: string): boolean {
    return existingCourses.some(course => course.name === newName);
  }

  addCourse(course:course) :Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.courseService.addCourse(this.course).subscribe({
        next: response => {
          if (response != null) {
            console.log(response);
            resolve();
          }
        },
        error: err => {
          this.toastService.stopLoader();
          console.log(err);
          reject();
        }
      });
    });
  }
}
