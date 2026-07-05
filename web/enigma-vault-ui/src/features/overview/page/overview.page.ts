import { Component, inject } from "@angular/core";
import { Router } from "@angular/router";

@Component({
    selector: 'overview-page',
    templateUrl: './overview.page.html',
    styleUrls: ['./overview.page.scss'],
    standalone: true
})
export class OverviewPage {
    private router = inject(Router);
    
    openPasswordsPage() {
        this.router.navigate(['/passwords']);
    }
}