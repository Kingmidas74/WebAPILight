import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../../environments/environment';
import { AppInterceptor } from '../app-interceptor';

import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { MaterialModule } from '../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { IdentityRoutes } from './identity-routing';



@NgModule({
  declarations: [LoginComponent, RegistrationComponent],
  imports: [
    CommonModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    RouterModule.forChild(IdentityRoutes),    
    MaterialModule,
    ReactiveFormsModule
  ]  
})
export class IdentityModule { }
