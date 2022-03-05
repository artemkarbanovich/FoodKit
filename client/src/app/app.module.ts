import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RegistrationComponent } from './core/components/registration/registration.component';
import { HomeComponent } from './core/components/home/home.component';
import { SignInComponent } from './core/components/sign-in/sign-in.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ProfileComponent } from './core/components/profile/profile.component';
import { PersonalDataComponent } from './core/components/personal-data/personal-data.component';
import { JwtInterceptor } from './core/security/interceptors/jwt.interceptor';
import { MomentDateModule } from '@angular/material-moment-adapter';
import { AddressComponent } from './core/components/address/address.component';
import { UserDishAddFormComponent } from './core/components/user-dish-add-form/user-dish-add-form.component';
import { UserDishTableComponent } from './core/components/user-dish-table/user-dish-table.component';
import { SharedModule } from './shared/modules/shared/shared.module';
import { IngredientAddFormComponent } from './core/components/admin/ingredient-add-form/ingredient-add-form.component';
import { IngredientTableComponent } from './core/components/admin/ingredient-table/ingredient-table.component';
import { DishAddFormComponent } from './core/components/admin/dish-add-form/dish-add-form.component';
import { DishComponent } from './core/components/admin/dish/dish.component';
import { DishListComponent } from './core/components/admin/dish-list/dish-list.component';
import { DishEditComponent } from './core/components/admin/dish-edit/dish-edit.component';
import { RouterModule } from '@angular/router';
import { DishMenuComponent } from './core/components/dish-menu/dish-menu.component';
import { OrderComponent } from './core/components/order/order.component';
import { UserOrdersComponent } from './core/components/user-orders/user-orders.component';
import { CancellationConfirmationComponent } from './shared/components/cancellation-confirmation/cancellation-confirmation.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    RegistrationComponent,
    HomeComponent,
    SignInComponent,
    ProfileComponent,
    PersonalDataComponent,
    AddressComponent,
    UserDishAddFormComponent,
    UserDishTableComponent,
    IngredientAddFormComponent,
    IngredientTableComponent,
    DishAddFormComponent,
    DishComponent,
    DishListComponent,
    DishEditComponent,
    DishMenuComponent,
    OrderComponent,
    UserOrdersComponent,
    CancellationConfirmationComponent
  ],
  imports: [
    SharedModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
    MomentDateModule,
    HttpClientModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
