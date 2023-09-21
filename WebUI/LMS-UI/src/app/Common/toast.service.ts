import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private toast: ToastrService, private ngxService: NgxUiLoaderService) { }

  showSuccess(msg: any) {
    this.toast.success(msg, 'Success');
  }

  showError(msg: any) {
    this.toast.error(msg, 'Error', { timeOut: 10000 });
  }

  showLoader() {
    this.ngxService.start();
  }

  stopLoader() {
    this.ngxService.stop();
  }
}
