import { inject } from "@angular/core";
import { CanActivateFn, Router, UrlTree } from "@angular/router";
import { CryptoStateService } from "../../../core/services/crypto-state.service";

export const passwordsGuard: CanActivateFn = (route, state): boolean | UrlTree => {
    const stateService = inject(CryptoStateService);
    const router = inject(Router);

    if (stateService.dek === null) {
        return router.createUrlTree(['/passwords/access'], {
            queryParams: { redirectTo: state.url }
        });
    }

    return true;
};