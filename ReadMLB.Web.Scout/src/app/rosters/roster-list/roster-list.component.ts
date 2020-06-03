import { Component, OnInit, Input } from '@angular/core';
import { PlayerLinkRendererComponent } from 'src/app/player-link-renderer.component';
import { environment } from 'src/environments/environment';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { PlayerModel } from 'src/app/players/player.model';

@Component({
  selector: 'app-roster-list',
  templateUrl: './roster-list.component.html',
  styleUrls: ['./roster-list.component.scss']
})
export class RosterListComponent implements OnInit {
  @Input() teamName: string;
  @Input() year: number;
  columnDefs = [];

  constructor(private route: ActivatedRoute, private http: HttpClient) { }
  teamId = this.route.snapshot.params.id;
  players: PlayerModel[];

  ngOnInit(): void {
    this.http.get<PlayerModel[]>(environment.API_URL + 'roster/' + this.teamId + '/' + this.year + '/false').subscribe(result => {
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
