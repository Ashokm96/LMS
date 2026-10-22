import { NgModule } from '@angular/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrModule } from 'ngx-toastr';
import { NgxUiLoaderModule } from 'ngx-ui-loader';

@NgModule({
  imports: [
    HttpClientTestingModule,
    ReactiveFormsModule,
    RouterTestingModule,
    MatDialogModule,
    ToastrModule.forRoot(),
    NgxUiLoaderModule
  ],
  exports: [
    HttpClientTestingModule,
    ReactiveFormsModule,
    RouterTestingModule,
    MatDialogModule,
    ToastrModule,
    NgxUiLoaderModule
  ],
  providers: [
    { provide: MAT_DIALOG_DATA, useValue: {} },
    { provide: MatDialogRef, useValue: { close: () => undefined } }
  ]
})
export class TestSupportModule {}
