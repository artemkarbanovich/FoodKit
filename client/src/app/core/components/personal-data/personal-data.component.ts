import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, Observable, take, throwError } from 'rxjs';
import { Account } from '../../models/account';
import { PersonalData } from '../../models/personal-data';
import { AccountService } from '../../services/account.service';
import { PersonalDataService } from '../../services/personal-data.service';

@Component({
  selector: 'app-personal-data',
  templateUrl: './personal-data.component.html',
  styleUrls: ['./personal-data.component.scss']
})
export class PersonalDataComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  private currentUserName: string;
  private isDateOfbirthReset: boolean | null = null;
  public userPersonalData$: Observable<PersonalData>;
  public personalDataForm: FormGroup;
  public minDateOfBirth: Date = new Date(new Date().getFullYear() - 100, 0, 1);
  public maxDateOfBirth: Date = new Date();
  public errors: string[] | null = null;

  constructor(private personalDataService: PersonalDataService, private formBuilder: FormBuilder,
    private accountService: AccountService, private toastr: ToastrService, private datePipe: DatePipe) { }


  public ngOnInit(): void {
    this.loadPersonalData();
  }

  public saveChanges(): void {
    let user: PersonalData = {
      userName: this.currentUserName,
      name: this.personalDataForm.controls['name'].value,
      phoneNumber: this.personalDataForm.controls['phoneNumber'].value,
      email: this.personalDataForm.controls['email'].value,
      dateOfBirth: !this.isDateOfbirthReset ? this.datePipe.transform(new Date(this.personalDataForm.controls['dateOfBirth'].value), 'yyyy-MM-dd') : null,
      gender: Number.parseInt(this.personalDataForm.controls['gender'].value) || null,
      bodyWeight: this.personalDataForm.controls['bodyWeight'].value || null,
      bodyHeight: this.personalDataForm.controls['bodyHeight'].value || null,
      physicalActivityCoefficient: Number.parseFloat(this.personalDataForm.controls['physicalActivityCoefficient'].value) || null
    };

    this.personalDataService.updatePersonalData(user)
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
        this.accountService.updateUserAfterChange(user.name, user.phoneNumber.replace('+', ''), user.phoneNumber, user.email);
        this.currentUserName = user.phoneNumber.replace("+", "");
        this.personalDataForm.markAsPristine();
        this.toastr.success('Изменения успешно сохранены');
      },
      error: (error) => console.log(error)
    });
  }

  public resetDateOfBirth(): void {
    this.personalDataForm.controls['dateOfBirth'].reset();
    this.isDateOfbirthReset = true;
    this.personalDataForm.markAsDirty();
  }

  private loadPersonalData(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe((user: Account) => {
      this.currentUserName = user.userName;
      this.userPersonalData$ =  this.personalDataService.getPersonalData(this.currentUserName)
      .pipe(map((user: PersonalData) => { 
        this.initializeForm(user);
        return user;
      }));
    });
  }

  private initializeForm(user: PersonalData): void {
    this.personalDataForm = this.formBuilder.group({
      name: [user.name, [Validators.required]],
      phoneNumber: [user.phoneNumber, [Validators.required,
        Validators.pattern('^(\\+375)(29|25|44|33)(\\d{3})(\\d{2})(\\d{2})$')]],
      email: [user.email, [Validators.required, Validators.email]],
      dateOfBirth: [user.dateOfBirth, []],
      gender: [user.gender?.toString(), []],
      bodyWeight: [user.bodyWeight, []],
      bodyHeight: [user.bodyHeight, []],
      physicalActivityCoefficient: [user.physicalActivityCoefficient?.toString(), []]
    });

    this.personalDataForm.controls['dateOfBirth'].valueChanges.subscribe(() => {
      if(this.isDateOfbirthReset && this.isDateOfbirthReset !== null) {
        this.isDateOfbirthReset = false;
      }
    });
    this.personalDataForm.controls['phoneNumber'].valueChanges.subscribe(() => {
      if(this.errors) {
        this.errors = null;
      }
    });
  }
}