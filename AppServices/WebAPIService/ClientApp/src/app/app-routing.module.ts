import { NgModule, ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule, } from '@angular/common';
import { BrowserModule  } from '@angular/platform-browser';
import { IdentityComponent } from './identity/identity/identity.component';
import { WorkspaceLayoutComponent } from './workspace/components/workspace-layout/workspace-layout.component';



export const routes: Routes = [  
  {
    path: '',
    redirectTo: 'workspace',
    pathMatch: 'full',
  },
  {
    path: 'identity',
    component: IdentityComponent,
    children: [{
      path: '',
      loadChildren: './identity/identity.module#IdentityModule',
    }]
  },
  {
    path: 'workspace',
    component: WorkspaceLayoutComponent,
    children: [{
      path: '',
      loadChildren: './workspace/workspace.module#WorkspaceModule',
    }]
  },
];

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(routes, {
       useHash: true
    })
  ],
  exports: [
  ],
})
export class AppRoutingModule { }
