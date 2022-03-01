import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DishEditComponent } from './core/components/admin/dish-edit/dish-edit.component';
import { DishComponent } from './core/components/admin/dish/dish.component';
import { IngredientTableComponent } from './core/components/admin/ingredient-table/ingredient-table.component';
import { DishMenuComponent } from './core/components/dish-menu/dish-menu.component';
import { HomeComponent } from './core/components/home/home.component';
import { ProfileComponent } from './core/components/profile/profile.component';
import { AdminGuard } from './core/security/guards/admin.guard';
import { AuthorizationGuard } from './core/security/guards/authorization.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthorizationGuard],
    children: [
      { path: 'profile', component: ProfileComponent },
      { path: 'ingredients', component: IngredientTableComponent, canActivate: [AdminGuard] },
      { path: 'dishes', component: DishComponent, canActivate: [AdminGuard] },
      { path: 'dishes/:id', component: DishEditComponent, canActivate: [AdminGuard] }
    ]
  },
  { path: 'menu', component: DishMenuComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
