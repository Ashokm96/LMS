import { TestBed } from '@angular/core/testing';
import { TestSupportModule } from '../testing/test-support.module';

import { CourseService } from './course.service';

describe('CourseService', () => {
  let service: CourseService;

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [TestSupportModule] });
    service = TestBed.inject(CourseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
