import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { PlayerModel } from '../player.model';
import { PlayerService } from '../players.service';
import { PlayerLinkRendererComponent } from 'src/app/player-link-renderer.component';
import { LinkRendererComponent } from 'src/app/link-renderer.component';

@Component({
  selector: 'app-player-search',
  templateUrl: './player-search.component.html',
  styleUrls: ['./player-search.component.scss']
})
export class PlayerSearchComponent implements OnInit {
  leagues =[0, 1, 2];
  filterForm: FormGroup;
  results: PlayerModel[];
  currentYear: number;
  columnDefs:  any[];
  constructor(private playersService: PlayerService) { }



  ngOnInit(): void {
    this.currentYear = +localStorage.getItem('currentYear');

    this.columnDefs = [
      {headerName: 'Team', cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: {route: '/teams', year: this.currentYear, idField: ['team', 'teamId'],
        valueField: ['team', 'teamAbr'] }}
      },
      {headerName: 'Org', cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: {route: '/teams', year: this.currentYear, idField: ['team', 'organizationId'],
        valueField: ['team', 'organization'] }}
      },
      {headerName: 'Player', cellRendererFramework: PlayerLinkRendererComponent,
      cellRendererParams: { inRouterLink: {route: '/players', year: this.currentYear }}
      },
      {headerName: 'Position', width: 100, valueGetter(params) {
        let retVal: string = params.data.primaryPosition;
        if (params.data.secondaryPosition && params.data.primaryPosition !== params.data.secondaryPosition) {
          retVal += ' / ' + params.data.secondaryPosition;
        }
        return retVal;
      }},
      {headerName: 'shirt#', field: 'shirt', width: 75, cellClass: 'text-right'},
      {headerName: 'B/T', width: 75, valueGetter(params) {
        return params.data.bats + '/' + params.data.throws;
      }}
    ];
    this.filterForm = new FormGroup({
      league: new FormControl(null),
      firstName: new FormControl(),
      lastName: new FormControl()
    });
  }

  onFilter(): void {

    this.playersService.searchPlayers(this.filterForm.get('firstName').value,
    this.filterForm.get('lastName').value,
    this.filterForm.get('league').value).subscribe( result => {
      this.results = result;
    });
  }

}
