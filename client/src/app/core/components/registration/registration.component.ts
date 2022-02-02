import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
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


  constructor(public dialogRef: MatDialogRef<RegistrationComponent>,
    private formBuilder: FormBuilder, private accountService: AccountService) { }

  
  public ngOnInit(): void {
    this.initializeForm();
  }

  public closeDialog(): void {
    this.dialogRef.close();
  }

  public register() : void {
    //TODO: service for register
  }

  private initializeForm() : void {
    this.registrationForm = this.formBuilder.group({
      name: ['', Validators.required],
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
  }

  private matchConfirmPasswordValue(passwordFormControl: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[passwordFormControl].value 
        ? null : { isMatching: true };
    }
  }
}