import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../../environments/environment';

import { MaterialModule } from '../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { WorkspaceRoutes } from './workspace-routing';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { WorkspaceLayoutComponent } from './components/workspace-layout/workspace-layout.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { StatisticsComponent } from './components/statistics/statistics.component';




@NgModule({
  declarations: [
    SidebarComponent, 
    NavbarComponent, 
    FooterComponent, 
    WorkspaceLayoutComponent, 
    DashboardComponent, 
    StatisticsComponent    
  ],
  imports: [
    CommonModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    RouterModule.forChild(WorkspaceRoutes),    
    MaterialModule,
    ReactiveFormsModule
  ],
  exports: [
    SidebarComponent,
    NavbarComponent,
    FooterComponent
  ]
})
export class WorkspaceModule { }
