import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatDialogModule } from '@angular/material/dialog';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Components/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { HomeComponent } from './Components/home/home.component';
import { AuthInterceptor } from './Common/auth.interceptor';
import { RegistrationComponent } from './Components/registration/registration.component';
import { AddCoursePopupComponent } from './dialogues/add-course-popup/add-course-popup.component';
import { DeleteCoursePopupComponent } from './dialogues/delete-course-popup/delete-course-popup.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegistrationComponent,
    AddCoursePopupComponent,
    DeleteCoursePopupComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot({
      //closeButton: true
    }),
    NgxUiLoaderModule,
    MatDialogModule
  ],
  providers: [{ provide: HTTP_INTERCEPTORS,useClass : AuthInterceptor,multi:true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
