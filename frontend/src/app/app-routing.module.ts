import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { RegisterComponent } from './pages/register/register.component';
import { authGuard } from './guards/auth.guard';

import { TodoListComponent } from './pages/todo-list/todo-list.component';
import { todoListGuard } from './guards/todo-list.guard';

const routes: Routes = [
  { path: 'todolist', component: TodoListComponent, canActivate:[todoListGuard], data: { roles:['Admin'] } },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: '', component: HomeComponent } // Ruta por defecto
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
