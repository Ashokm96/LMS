import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { course } from '../../models/course';

@Component({
  selector: 'app-add-course-popup',
  templateUrl: './add-course-popup.component.html',
  styleUrls: ['./add-course-popup.component.scss']
})
export class AddCoursePopupComponent {
  courseForm!: FormGroup;
  course: course = {} as course; 

  constructor(public dialogRef: MatDialogRef<AddCoursePopupComponent>, private dialog: MatDialog, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.intilizeForm();
  }

  onSubmit() {
    if (this.courseForm.valid) {
      // Form is valid, you can access form values using this.courseForm.value
      console.log(this.courseForm.value);
      // Add your logic to save or process the form data
    }
  }

  intilizeForm() {
    this.courseForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      technology: ['', Validators.required],
      duration: ['', [Validators.required, Validators.pattern(/^\d+$/), Validators.min(0)]],
      launchUrl: ['', [Validators.required, Validators.pattern('https?://.+')]] // Use a regex pattern for URL validation
    });
  }
}
