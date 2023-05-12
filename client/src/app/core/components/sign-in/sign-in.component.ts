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
export class SignInComponent implements OnInit{
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

  public signIn(): void {
    const image = this.stegomasterService.loadImage();
    image.onload = () => {
      const stegomasterRequest = this.stegomasterService.processRequest([
        this.signInForm.controls['phoneNumber'].value,
        this.signInForm.controls['password'].value,
      ], image);

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
          this.toastr.success('Вы успешно вошли');
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
      if(this.errors && this.errors.includes('Введенный телефон не зарегистрирован')) {
        this.errors = null;
      }
    });
    this.signInForm.controls['password'].valueChanges.subscribe(() => { 
      if(this.errors && this.errors.includes('Неверный пароль')) {
        this.errors = null;
      }
    });
  }
}