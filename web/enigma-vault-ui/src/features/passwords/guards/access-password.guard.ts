import { CanActivateFn, Router, UrlTree } from "@angular/router";
import { CryptoStateService } from "../../../core/services/crypto-state.service";
import { inject } from "@angular/core";

export const accessPasswordGuard: CanActivateFn = (route, state): boolean | UrlTree => {
    const stateService = inject(CryptoStateService);
    const router = inject(Router);

    if (stateService.dek !== null) {
        return router.createUrlTree(['/passwords'], {
            queryParams: { from: state.url }
        });
    }

    return true;
};