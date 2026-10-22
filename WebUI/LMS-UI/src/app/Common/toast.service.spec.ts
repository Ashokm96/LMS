import { TestBed } from '@angular/core/testing';
import { TestSupportModule } from '../testing/test-support.module';

import { ToastService } from './toast.service';

describe('ToastService', () => {
  let service: ToastService;

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [TestSupportModule] });
    service = TestBed.inject(ToastService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
