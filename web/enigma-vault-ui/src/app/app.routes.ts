import { Routes } from '@angular/router';
import { MainLayoutComponent } from '../core/layouts/main/main-layout.component';
import { OverviewPage } from '../features/overview/page/overview.page';
import { PasswordsPage } from '../features/passwords/pages/passwords/passwords.page';

export const routes: Routes = [
    { path: '', redirectTo: '/overview', pathMatch: 'full' },
    { 
        path: '',
        loadComponent: () => MainLayoutComponent,
        children: [
            { path: 'overview', loadComponent: () => OverviewPage },
            { path: 'passwords', loadComponent: () => PasswordsPage }
        ]
    }
];
