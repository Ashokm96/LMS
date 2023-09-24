import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { course } from '../../models/course';

@Component({
  selector: 'app-add-course-popup',
  templateUrl: './add-course-popup.component.html',
  styleUrls: ['./add-course-popup.component.scss']
})
export class AddCoursePopupComponent {
  course: course = {} as course; 

  constructor(public dialogRef : MatDialogRef<AddCoursePopupComponent>,private dialog:MatDialog) { }

  onSubmit() {
    // You can perform actions with the submitted data here
    console.log('Form submitted with data:', this.course);

    // Reset the form after submission (optional)
    //this.course = {} ;
  }
}
