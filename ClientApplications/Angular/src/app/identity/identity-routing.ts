import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';



export const IdentityRoutes: Routes = [
    { path: '',      component: LoginComponent },
    { path: 'login',      component: LoginComponent },
    { path: 'registration',   component: RegistrationComponent },
];