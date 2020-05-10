import { NgModule, ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule, } from '@angular/common';
import { BrowserModule  } from '@angular/platform-browser';
import { IdentityComponent } from './identity/identity/identity.component';
import { WorkspaceLayoutComponent } from './workspace/components/workspace-layout/workspace-layout.component';
import { CanLoadWorkspace } from './workspace/can-load-workspace';
import { WorkspaceModule } from './workspace/workspace.module';
import { CanLoadIdentity } from './identity/can-load-identity';



export const routes: Routes = [  
  {
    path: '',
    redirectTo: 'workspace',
    pathMatch: 'full',
  },
  {
    path: 'identity',
    component: IdentityComponent,    
    canActivateChild:[CanLoadIdentity],
    children: [{
      path: '',
      loadChildren: './identity/identity.module#IdentityModule',
    }]
  },
  {
    path: 'workspace',
    component: WorkspaceLayoutComponent,
    canLoad:[CanLoadWorkspace],
    canActivateChild:[CanLoadWorkspace],
    children: [{
      path: '',      
      canLoad:[CanLoadWorkspace],
      loadChildren: './workspace/workspace.module#WorkspaceModule',
    }]
  },
];

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    WorkspaceModule,
    RouterModule.forRoot(routes, {
       useHash: true
    })
  ],
  exports: [
  ],
  providers: [CanLoadWorkspace, CanLoadIdentity]
})
export class AppRoutingModule { }
