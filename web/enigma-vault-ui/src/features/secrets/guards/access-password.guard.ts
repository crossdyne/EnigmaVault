import { CanActivateFn, Router, UrlTree } from "@angular/router";
import { inject } from "@angular/core";
import { CryptoWorkerService } from "../../../core/services/crypto-worker.service";

export const accessPasswordGuard: CanActivateFn = (route, state): boolean | UrlTree => {
    const cryptoWorker = inject(CryptoWorkerService);
    const router = inject(Router);

    if (cryptoWorker.initialized) {
        return router.createUrlTree(['/passwords'], {
            queryParams: { from: state.url }
        });
    }

    return true;
};