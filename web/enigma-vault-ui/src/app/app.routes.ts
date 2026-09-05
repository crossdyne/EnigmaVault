import { Routes } from '@angular/router';
import { MainLayoutComponent } from '../core/layouts/main/main-layout.component';
import { PasswordsPageComponent } from '../features/secrets/pages/passwords/passwords-page.component';
import { accessPasswordGuard } from '../features/secrets/guards/access-password.guard';
import { passwordsGuard } from '../features/secrets/guards/passwords.guard';
import { InputPasswordPageComponent } from '../features/secrets/pages/input-password/input-password-page.component';
import { OverviewPageComponent } from '../features/overview/pages/overview/overview-page.component';

export const routes: Routes = [
    { path: '', redirectTo: '/overview', pathMatch: 'full' },
    { 
        path: '',
        loadComponent: () => MainLayoutComponent,
        children: [
            { path: 'overview', loadComponent: () => OverviewPageComponent },
            { path: 'passwords', loadComponent: () => PasswordsPageComponent, canActivate: [passwordsGuard] },
            { path: 'passwords/access', loadComponent: () => InputPasswordPageComponent, canActivate: [accessPasswordGuard] },
        ]
    }
];
