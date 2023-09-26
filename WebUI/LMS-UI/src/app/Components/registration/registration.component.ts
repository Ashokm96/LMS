import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastService } from '../../Common/toast.service';
import { user } from '../../models/user';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent {
  registrationForm !: FormGroup;
  user: user = {} as user;
  errorLabel!: boolean;
  successLabel!: boolean;
  notifyMessage: any;
  constructor(private fb: FormBuilder,private courseService:CourseService,private toast:ToastService,private router:Router) {   }

  ngOnInit(): void {
    this.initilizeForm();
  }

  initilizeForm() {
    this.registrationForm = this.fb.group({
      //userId: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/)]],
      confirmPassword: ['', Validators.required]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  passwordMatchValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      return { 'passwordMismatch': true };
    }
    return null;
  }


  onSubmit() {
    this.toast.showLoader();
    if (this.registrationForm.valid) {
      // Perform registration logic here
      this.user.userName = this.registrationForm.get('userName')?.value;
      this.user.email = this.registrationForm.get('email')?.value;
      this.user.password = this.registrationForm.get('password')?.value;
      this.user.confirmpassword = this.registrationForm.get('confirmPassword')?.value;
      this.user.role = "User";

      this.courseService.registerUser(this.user).subscribe({
        next: response => {
          if (response.message =="User details already exists.") {
            this.showError(response.message);
          }
          else if (response.message == "Failed to register") {
            this.showError(response.message);
          }
          else if (response.message == "An error occured. contact your system administrator.") {
            this.showError(response.message);
          }
          else if (response.message == "Registered succefully") {
            this.showSuccess("Registered succefully");
          }
          this.toast.stopLoader();
          this.registrationForm.reset();
        },
        error: err => {
          this.toast.stopLoader();
          this.showError(err.error.message);
        }
      });
    }
  }

  closeNotify() {
    this.errorLabel = false;
    this.notifyMessage = null;
    this.successLabel = false;
  }

  showError(errMsg: string) {
    this.errorLabel = true;
    this.notifyMessage = errMsg;
    setTimeout(() => {
      this.closeNotify();
    }, 8000);
  }

  showSuccess(errMsg: string) {
    this.successLabel = true;
    this.notifyMessage = errMsg;
    setTimeout(() => {
      this.closeNotify();
    }, 8000);
  }


}

