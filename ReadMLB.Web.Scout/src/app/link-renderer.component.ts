import { Component } from '@angular/core';
import { AgRendererComponent } from 'ag-grid-angular';

@Component({
    template: `<a [routerLink]="[params.inRouterLink.route, params.inRouterLink.idField.length ?
    params.data[params.inRouterLink.idField[0]][params.inRouterLink.idField[1]]
    : params.data[params.inRouterLink.idField]]"
      [queryParams]="{year: params.inRouterLink.year}">
      {{params.inRouterLink.valueField.length ?
        params.data[params.inRouterLink.valueField[0]][params.inRouterLink.valueField[1]]
        : params.data[params.inRouterLink.valueField]
      }}
      </a>`
})
export class LinkRendererComponent implements AgRendererComponent {
    params: any;

    agInit(params: any): void {
        this.params = params;
    }

    refresh(params: any): boolean {
        return false;
    }
}
