import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminChatComponent } from './core/components/admin/admin-chat/admin-chat.component';
import { DishEditComponent } from './core/components/admin/dish-edit/dish-edit.component';
import { DishComponent } from './core/components/admin/dish/dish.component';
import { IngredientTableComponent } from './core/components/admin/ingredient-table/ingredient-table.component';
import { MessageListComponent } from './core/components/admin/message-list/message-list.component';
import { DishMenuComponent } from './core/components/dish-menu/dish-menu.component';
import { HomeComponent } from './core/components/home/home.component';
import { OrderComponent } from './core/components/order/order.component';
import { ProfileComponent } from './core/components/profile/profile.component';
import { AdminGuard } from './core/security/guards/admin.guard';
import { AuthorizationGuard } from './core/security/guards/authorization.guard';
import { ChatGuard } from './core/security/guards/chat.guard';
import { OrderGuard } from './core/security/guards/order.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthorizationGuard],
    children: [
      { path: 'profile', component: ProfileComponent },
      { path: 'order', component: OrderComponent, canActivate: [OrderGuard] },
      { path: 'ingredients', component: IngredientTableComponent, canActivate: [AdminGuard] },
      { path: 'dishes', component: DishComponent, canActivate: [AdminGuard] },
      { path: 'dishes/:id', component: DishEditComponent, canActivate: [AdminGuard] },
      { path: 'chat', component: MessageListComponent, canActivate: [AdminGuard] },
      { path: 'chat/:id', component: AdminChatComponent, canActivate: [AdminGuard, ChatGuard] }
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
