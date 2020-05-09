import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { StatisticsComponent } from './components/statistics/statistics.component';



export const WorkspaceRoutes: Routes = [
    { path: '',      component: DashboardComponent },
    { path: 'dashboard',      component: DashboardComponent },
    { path: 'statistics',     component: StatisticsComponent}
];