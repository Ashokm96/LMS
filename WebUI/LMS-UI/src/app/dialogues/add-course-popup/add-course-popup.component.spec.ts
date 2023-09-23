import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCoursePopupComponent } from './add-course-popup.component';

describe('AddCoursePopupComponent', () => {
  let component: AddCoursePopupComponent;
  let fixture: ComponentFixture<AddCoursePopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddCoursePopupComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCoursePopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
