import { Component, EventEmitter, Output } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-course-popup',
  templateUrl: './delete-course-popup.component.html',
  styleUrls: ['./delete-course-popup.component.scss']
})
export class DeleteCoursePopupComponent {
  @Output() confirmed = new EventEmitter<boolean>();

  constructor(public dialogRef: MatDialogRef<DeleteCoursePopupComponent>, private dialog: MatDialog) { }

  confirmDelete() {
    this.confirmed.emit(true);
  }

  cancelDelete() {
    this.confirmed.emit(false);
  }
}
