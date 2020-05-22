import { Component } from '@angular/core';
import { AgRendererComponent } from 'ag-grid-angular';

@Component({
    template: '<a [routerLink]="[params.inRouterLink,params.data.playerId]">{{params.data.firstName}} {{params.data.lastName}}</a>'
})
export class PlayerLinkRendererComponent implements AgRendererComponent {
    params: any;

    agInit(params: any): void {
        this.params = params;
    }

    refresh(params: any): boolean {
        return false;
    }
}
