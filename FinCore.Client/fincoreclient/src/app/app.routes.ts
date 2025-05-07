import { Routes } from '@angular/router';
import { LoginFormComponent } from './login-form/login-form.component';
import { RegisterFormComponent } from './register-form/register-form.component';
import { MainPageComponent } from './main-page/main-page.component';
import {PaymentFormComponent} from './payment-form/payment-form.component';
export const routes: Routes = [
  { path: 'login', component: LoginFormComponent },
  { path: 'register', component: RegisterFormComponent },
  { path: 'main', component: MainPageComponent },
  { path: 'payments/:userId', component: PaymentFormComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
