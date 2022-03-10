import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { DeletionConfirmationComponent } from '../../components/deletion-confirmation/deletion-confirmation.component';
import { ToastrModule } from 'ngx-toastr';
import { MaterialModule } from '../material/material.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from 'src/app/core/security/interceptors/loading.interceptor';
import { FileUploadModule } from "ng2-file-upload"; 
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgxStarRatingModule } from 'ngx-star-rating';
import { CancellationConfirmationComponent } from '../../components/cancellation-confirmation/cancellation-confirmation.component';

@NgModule({
  declarations: [
    DeletionConfirmationComponent,
    CancellationConfirmationComponent
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
    NgxSpinnerModule,
    FileUploadModule,
    NgxStarRatingModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
    DatePipe
  ],
})
export class SharedModule { }
