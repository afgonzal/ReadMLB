import { Component, OnInit } from '@angular/core';
import { PlayerLinkRendererComponent } from 'src/app/player-link-renderer.component';
import { environment } from 'src/environments/environment';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-roster-list',
  templateUrl: './roster-list.component.html',
  styleUrls: ['./roster-list.component.scss']
})
export class RosterListComponent implements OnInit {
  columnDefs = [
    {headerName: 'Player', cellRendererFramework: PlayerLinkRendererComponent,
    cellRendererParams: { inRouterLink: '/players' }
    },
    {headerName: 'Position', width: 100, valueGetter: function(params){
      let retVal: string = params.data.primaryPosition;
      if (params.data.secondaryPosition && params.data.primaryPosition !== params.data.secondaryPosition) {
        retVal += ' / ' + params.data.secondaryPosition;
      }
      return retVal;
    }},
    {headerName: 'shirt#', field: 'shirt', width: 75, cellClass: 'text-right'},
    {headerName: 'B/T', width: 75, valueGetter: function(params) {
      return params.data.bats + '/' + params.data.throws;
    }}
  ];

  constructor(private route: ActivatedRoute, private http: HttpClient) { }
  teamId = this.route.snapshot.params.id;
  players = [];

  ngOnInit(): void {
    this.http.get<any[]>(environment.API_URL + 'roster/' + this.teamId + '/2008/false').subscribe(result => {
      this.players = result;
    });
  }
}
