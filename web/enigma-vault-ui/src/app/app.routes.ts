import { Routes } from '@angular/router';
import { MainLayoutComponent } from '../core/layouts/main/main-layout.component';
import { OverviewPage } from '../features/overview/page/overview.page';
import { PasswordsPage } from '../features/secrets/pages/passwords/passwords-page.component';
import { accessPasswordGuard } from '../features/secrets/guards/access-password.guard';
import { passwordsGuard } from '../features/secrets/guards/passwords.guard';
import { InputPasswordPage } from '../features/secrets/pages/input-password/input-password-page.component';

export const routes: Routes = [
    { path: '', redirectTo: '/overview', pathMatch: 'full' },
    { 
        path: '',
        loadComponent: () => MainLayoutComponent,
        children: [
            { path: 'overview', loadComponent: () => OverviewPage },
            { path: 'passwords', loadComponent: () => PasswordsPage, canActivate: [passwordsGuard] },
            { path: 'passwords/access', loadComponent: () => InputPasswordPage, canActivate: [accessPasswordGuard] },
        ]
    }
];
