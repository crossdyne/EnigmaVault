import { Routes } from '@angular/router';
import { MainLayoutComponent } from '../core/layouts/main/main-layout.component';
import { OverviewPage } from '../features/overview/page/overview.page';

export const routes: Routes = [
    { path: '', redirectTo: '/overview', pathMatch: 'full' },
    { 
        path: 'overview',
        loadComponent: () => MainLayoutComponent,
        children: [
            { path: '', loadComponent: () => OverviewPage }
        ]
    }
];
