import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DishComponent } from './core/components/admin/dish/dish.component';
import { IngredientTableComponent } from './core/components/admin/ingredient-table/ingredient-table.component';
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
      { path: 'dishes', component: DishComponent, canActivate: [AdminGuard] }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
