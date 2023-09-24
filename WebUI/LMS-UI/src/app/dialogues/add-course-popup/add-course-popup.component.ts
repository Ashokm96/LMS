import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
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

  constructor(public dialogRef: MatDialogRef<AddCoursePopupComponent>, private dialog: MatDialog, private fb: FormBuilder,private courseService:CourseService) { }

  ngOnInit(): void {
    this.intilizeForm();
  }

  onSubmit() {
    if (this.courseForm.valid) {
      this.course.name = this.courseForm.get('name')?.value;
      this.course.technology = this.courseForm.get('technology')?.value;
      this.course.description = this.courseForm.get('description')?.value;
      this.course.duration = this.courseForm.get('duration')?.value;
      this.course.launchUrl = this.courseForm.get('launchUrl')?.value;

      this.courseService.addCourse(this.course).subscribe(
        (response) => {
          console.log(response);
        },
        (error) => {
          console.log(error.error.message);
        }
      );
    }
  }

  intilizeForm() {
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

}
