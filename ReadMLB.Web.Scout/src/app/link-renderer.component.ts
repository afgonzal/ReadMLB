import { Component } from '@angular/core';
import { AgRendererComponent } from 'ag-grid-angular';

@Component({
    template: `<a [routerLink]="[params.inRouterLink.route, idFieldIsArray ?
    params.data[params.inRouterLink.idField[0]][params.inRouterLink.idField[1]]
    : params.data[params.inRouterLink.idField]]"
      [queryParams]="{year: params.inRouterLink.year}">
      {{ valueFieldIsArray ?
        params.data[params.inRouterLink.valueField[0]][params.inRouterLink.valueField[1]]
        : params.data[params.inRouterLink.valueField]
      }}
      </a>`
})
export class LinkRendererComponent implements AgRendererComponent {
    params: any;
    idFieldIsArray = false;
    valueFieldIsArray = false;

    agInit(params: any): void {
        this.params = params;
        if (this.params.inRouterLink.idField instanceof Array) {
          this.idFieldIsArray = true;
        }
        if (this.params.inRouterLink.valueField instanceof Array) {
          this.valueFieldIsArray = true;
        }
    }

    refresh(params: any): boolean {
        return false;
    }
}
