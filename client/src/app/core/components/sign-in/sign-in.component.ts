import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';
import { AccountService } from '../../services/account.service';
import { StegomasterService } from 'src/app/stegomaster/stegomaster.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
  public signInForm: FormGroup;
  public hidePassword: boolean = true;
  public errors: string[] | null = null;


  constructor(private accountService: AccountService, private toastr: ToastrService,
    private dialogRef: MatDialogRef<SignInComponent>, private formBuilder: FormBuilder,
    private stegomasterService: StegomasterService) { }


  public ngOnInit(): void {
    this.initializeForm();
  }

  public closeDialog(): void {
    this.dialogRef.close();
  }

  public async signIn(): Promise<void> {
    await this.logInfo(['1. Login operation is started...']);

    const data: string[] = [
      this.signInForm.controls['phoneNumber'].value,
      this.signInForm.controls['password'].value,
    ];
    await this.logInfo(['2. User entered next data:', '\n\t', data[0], '\n\t', data[1]])

    const image = await this.stegomasterService.loadImageToRequest(data);
    image.onload = async () => {
      const stegomasterRequest = await this.stegomasterService.processRequest(data, image);

      await this.logInfo(['10. Login request is sent to the server...']);
      this.accountService.signIn(stegomasterRequest)
      .pipe(
        catchError(error => {
          if(error) {
            this.errors = [];
            if(error.error.errors) {
              for(const key in error.error.errors) {
                this.errors.push(error.error.errors[key]);
              } 
            } else {
              this.errors.push(error.error);
            }
          }
          return throwError(() => error);
        })
      )
      .subscribe({
        complete: () => {
          this.closeDialog();
          this.toastr.success('You successfully login');
        },
        error: (error) => console.log(error)
      });
    }
  }

  private initializeForm() : void {
    this.signInForm = this.formBuilder.group({
      phoneNumber: ['', [Validators.required,
        Validators.pattern('^(\\+375)(29|25|44|33)(\\d{3})(\\d{2})(\\d{2})$')]],
      password: ['', [Validators.required,
        Validators.minLength(6),
        Validators.maxLength(32)]]
    });

    this.signInForm.controls['phoneNumber'].valueChanges.subscribe(() => {
      if(this.errors && this.errors.includes('Phone number is not registered')) {
        this.errors = null;
      }
    });
    this.signInForm.controls['password'].valueChanges.subscribe(() => { 
      if(this.errors && this.errors.includes('Invalid password')) {
        this.errors = null;
      }
    });
  }

  private async logInfo(info: string[]): Promise<void> {
    await this.stegomasterService.logInfo(info);
  }
}