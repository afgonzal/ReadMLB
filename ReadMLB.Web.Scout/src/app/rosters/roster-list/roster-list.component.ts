import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { PlayerLinkRendererComponent } from 'src/app/player-link-renderer.component';
import { ActivatedRoute } from '@angular/router';
import { PlayerModel } from 'src/app/players/player.model';
import { TeamsService } from '../../teams/teams.service';

@Component({
  selector: 'app-roster-list',
  templateUrl: './roster-list.component.html',
  styleUrls: ['./roster-list.component.scss']
})
export class RosterListComponent implements OnChanges{
  @Input() teamId: number;
  @Input() teamName: string;
  @Input() year: number;
  columnDefs = [];

  constructor(private route: ActivatedRoute, private teamsService: TeamsService) { }
  players: PlayerModel[];

  ngOnChanges() {
    this.loadRoster();
  }
  loadRoster(): void {
    this.teamsService.getRoster(this.teamId, this.year).subscribe(result => {
      this.columnDefs = [
        {headerName: 'Player', cellRendererFramework: PlayerLinkRendererComponent,
        cellRendererParams: { inRouterLink: {route: '/players', year: this.year }}
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
      this.players = result;
    });
  }
}
