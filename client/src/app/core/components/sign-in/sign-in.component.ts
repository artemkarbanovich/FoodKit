import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit{
  public signInForm: FormGroup;
  public hidePassword: boolean = true;


  constructor(private dialogRef: MatDialogRef<SignInComponent>, private formBuilder: FormBuilder) { }


  public ngOnInit(): void {
    this.initializeForm();
  }

  public closeDialog() {
    this.dialogRef.close();
  }

  public signIn() {
    //TODO: service for sign in
  }

  private initializeForm() : void {
    this.signInForm = this.formBuilder.group({
      phoneNumber: ['', [Validators.required,
        Validators.pattern('^(\\+375)(29|25|44|33)(\\d{3})(\\d{2})(\\d{2})$')]],
      password: ['', [Validators.required,
        Validators.minLength(6),
        Validators.maxLength(32)]]
    });
  }
}