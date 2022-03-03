import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Address } from '../../models/address';
import { AddressService } from '../../services/address.service';
import { MatDialog } from '@angular/material/dialog';
import { DeletionConfirmationComponent } from '../../../shared/components/deletion-confirmation/deletion-confirmation.component';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.scss']
})
export class AddressComponent implements OnInit {
  @Output() updateAddresses = new EventEmitter<void>();
  public panelExpansionState: boolean = false;
  public addressForm: FormGroup;
  public addresses: Address[];

  constructor(private addressService: AddressService, private toastr: ToastrService,
    private formBuilder: FormBuilder, private dialog: MatDialog) { }


  public ngOnInit(): void {
    this.loadAddresses();
    this.initializeForm();
  }

  public deleteAddress(address: Address): void {
    this.dialog.open(DeletionConfirmationComponent, {disableClose: true, autoFocus: false})
    .afterClosed().subscribe(result => {
      if(result == false) return;

      this.addressService.deleteAddress(address.id).subscribe({
        complete: () => {
          this.addresses.splice(this.addresses.indexOf(address), 1);
          this.toastr.success('Адрес успешно удален');
        },
        error: () => this.toastr.error('Ошибка удаления')
      });
    });
  }

  public addAddress(editForm: FormGroupDirective): void {
    let address: Address = {
      country: this.addressForm.controls['country'].value,
      locality: this.addressForm.controls['locality'].value,
      street: this.addressForm.controls['street'].value,
      houseNumber: this.addressForm.controls['houseNumber'].value,
      apartmentNumber: this.addressForm.controls['apartmentNumber'].value,
      entranceNumber: this.addressForm.controls['entranceNumber'].value || null,
      floor: this.addressForm.controls['floor'].value || null,
    };

    this.addressService.addAddress(address).subscribe((addressId: number) => {
      address.id = addressId;
      this.addresses.push(address);
      this.toastr.success('Адрес упешно добавлен');
      editForm.resetForm();
      this.addressForm.reset();
      this.panelExpansionState = !this.panelExpansionState;
      this.updateAddresses.next();
    });
  }

  private loadAddresses(): void {
    this.addressService.getAddresses().subscribe(((addresses: Address[]) => {
      this.addresses = addresses;
    }));
  }

  private initializeForm(): void {
    this.addressForm = this.formBuilder.group({
      country: ['', [Validators.required, Validators.maxLength(50)]],
      locality: ['', [Validators.required, Validators.maxLength(50)]],
      street: ['', [Validators.required, Validators.maxLength(50)]],
      houseNumber: ['', [Validators.required, Validators.maxLength(10)]],
      apartmentNumber: ['', [Validators.required, Validators.maxLength(10)]],
      entranceNumber: ['', [Validators.maxLength(5)]],
      floor: ['', [Validators.maxLength(5)]]
    });
  }
}