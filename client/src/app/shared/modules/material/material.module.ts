import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatNativeDateModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatSliderModule } from '@angular/material/slider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatBadgeModule } from '@angular/material/badge';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSortModule } from '@angular/material/sort';
import { MatStepperModule } from '@angular/material/stepper';
import { MatCardModule } from '@angular/material/card';
import { MatCarouselModule } from 'ng-mat-carousel';
import { MatRadioModule } from '@angular/material/radio';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { DATE_FORMATS } from 'src/app/core/constants/date-formats';
import { 
  NgxMatDateAdapter,
  NgxMatDatetimePickerModule, 
  NgxMatNativeDateModule, 
  NgxMatTimepickerModule 
} from '@angular-material-components/datetime-picker';
import { NGX_MAT_DATE_FORMATS } from '@angular-material-components/datetime-picker';
import { DATE_TIME_FORMATS } from 'src/app/core/constants/date-time-formats';
import { CustomNgxDatetimeAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from 'src/app/core/helpers/customNgxDatetimeAdapter';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatCarouselModule.forRoot()
  ],
  exports: [
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatTabsModule,
    MatToolbarModule,
    MatDividerModule,
    MatExpansionModule,
    MatSortModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatBadgeModule,
    MatSelectModule,
    MatListModule,
    MatPaginatorModule,
    MatCheckboxModule,
    MatSliderModule,
    MatTooltipModule,
    MatTableModule,
    MatProgressSpinnerModule,
    MatStepperModule,
    MatCardModule,
    MatCarouselModule,
    MatRadioModule,
    NgxMatDatetimePickerModule,
    NgxMatNativeDateModule,
    NgxMatTimepickerModule,
    DragDropModule
  ],
  providers: [
    { provide: MAT_DATE_FORMATS, useValue: DATE_FORMATS },
    {
      provide: NgxMatDateAdapter,
      useClass: CustomNgxDatetimeAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },
    { provide: NGX_MAT_DATE_FORMATS, useValue: DATE_TIME_FORMATS }
  ]
})
export class MaterialModule { }
