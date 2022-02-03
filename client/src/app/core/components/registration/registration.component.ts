import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';
import { Register } from '../../models/register';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
  public registrationForm: FormGroup;
  public hidePassword: boolean = true;
  public hideConfirmPassword: boolean = true;
  public errors: string[] | null = null;


  constructor(private dialogRef: MatDialogRef<RegistrationComponent>, private toastr: ToastrService,
    private formBuilder: FormBuilder, private accountService: AccountService) { }

  
  public ngOnInit(): void {
    this.initializeForm();
  }

  public closeDialog(): void {
    this.dialogRef.close();
  }

  public register() : void {
    let user: Register = {
      name: this.registrationForm.controls['name'].value,
      phoneNumber: this.registrationForm.controls['phoneNumber'].value,
      email: this.registrationForm.controls['email'].value,
      password: this.registrationForm.controls['password'].value
    };

    this.accountService.register(user)
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
        this.toastr.success('Вы успешно зарегистрировались и вошли');
      },
      error: (error) => console.log(error)
    });
  }

  private initializeForm() : void {
    this.registrationForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required,
        Validators.pattern('^(\\+375)(29|25|44|33)(\\d{3})(\\d{2})(\\d{2})$')]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required,
        Validators.minLength(6),
        Validators.maxLength(32)]],
      confirmPassword: ['', [Validators.required, 
        this.matchConfirmPasswordValue('password')]]
    });

    this.registrationForm.controls['password'].valueChanges.subscribe(() => {
      this.registrationForm.controls['confirmPassword'].updateValueAndValidity();
    });
    this.registrationForm.controls['phoneNumber'].valueChanges.subscribe(() => {
      if(this.errors) {
        this.errors = null;
      }
    });
  }

  private matchConfirmPasswordValue(passwordFormControl: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[passwordFormControl].value 
        ? null : { isMatching: true };
    }
  }
}