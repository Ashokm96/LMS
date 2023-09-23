import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-add-course-popup',
  templateUrl: './add-course-popup.component.html',
  styleUrls: ['./add-course-popup.component.scss']
})
export class AddCoursePopupComponent {
  @Input() showPopup!: boolean;
  @Output() addCourse = new EventEmitter<any>();

  newCourse = {
    name: '',
    description: '',
    technology: '',
    duration: 0,
    launchUrl: ''
  };

  addNewCourse() {
    // Emit an event to add the new course
    this.addCourse.emit(this.newCourse);

    // Reset the form and hide the popup
    this.newCourse = {
      name: '',
      description: '',
      technology: '',
      duration: 0,
      launchUrl: ''
    };
    this.showPopup = false;
  }
}
