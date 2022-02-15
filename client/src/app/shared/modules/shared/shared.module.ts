import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { DeletionConfirmationComponent } from '../../components/deletion-confirmation/deletion-confirmation.component';
import { ToastrModule } from 'ngx-toastr';
import { MaterialModule } from '../material/material.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from 'src/app/core/security/interceptors/loading.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

@NgModule({
  declarations: [
    DeletionConfirmationComponent
  ],
  imports: [
    MaterialModule,
    CommonModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-left'
    })
  ],
  exports: [
    MaterialModule,
    NgxSpinnerModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
    DatePipe
  ],
})
export class SharedModule { }
