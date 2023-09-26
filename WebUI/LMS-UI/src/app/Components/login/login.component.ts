import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { ToastService } from '../../Common/toast.service';
import { login } from '../../models/login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  error: any;
  constructor(private authService:AuthService,private router:Router,private toast:ToastService) { }

  loginForm = new FormGroup({
    Username:new FormControl('',Validators.required),
    password:new FormControl('',Validators.required)
  });

  ngOnInit(): void {
    this.authService.isLoggedIn();
  }

  onSubmit(): void {
    this.error = null;
    this.toast.showLoader();
    let cred = this.loginForm.value;
    let loginUser = new login(cred.Username!, cred.password!);

    // If the credentials match, log in the user
    this.authService.login(loginUser).subscribe(
      (res) => {
        this.toast.stopLoader();
        if (res.message == "Username/Password incorrect.") {
          this.error = res.message;
        }
        else {
          //this.toast.showSuccess('Login Successful');

          // Storing the token in local storage
          localStorage.setItem('token', res.token);

          // Redirect based on user role
          if (this.authService.isAdmin()) {
            this.router.navigate(['admin']);
          } else {
            this.router.navigate(['user']);
          }
        }
      },
      (error) => {
        this.toast.stopLoader();
        if (error.error.message == "Username/Password incorrect.") {
          this.error = error.error.message;
        } else {
          this.toast.showError(error.error.message);
        }
      }
    );
  }

  emptyError() {
    this.error = '';
  }
}
