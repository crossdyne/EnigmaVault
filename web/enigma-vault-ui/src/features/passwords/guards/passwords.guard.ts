import { inject } from "@angular/core";
import { CanActivateFn, Router, UrlTree } from "@angular/router";
import { CryptoWorkerService } from "../../../core/services/crypto-worker.service";

export const passwordsGuard: CanActivateFn = (route, state): boolean | UrlTree => {
    const cryptoWorker = inject(CryptoWorkerService);
    const router = inject(Router);

    if (!cryptoWorker.initialized) {
        return router.createUrlTree(['/passwords/access'], {
            queryParams: { redirectTo: state.url }
        });
    }

    return true;
};