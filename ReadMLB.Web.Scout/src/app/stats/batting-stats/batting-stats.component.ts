import { Component, OnInit, Input } from '@angular/core';
import { BattingStatModel } from 'src/app/batter/battingStat.model';
import { LeaguePipe } from 'src/app/teams/league-pipe.pipe';
import { StatsService } from '../stats.service';
import { PlayerLinkRendererComponent } from 'src/app/player-link-renderer.component';
import { LinkRendererComponent } from 'src/app/link-renderer.component';

@Component({
  selector: 'app-batting-stats',
  templateUrl: './batting-stats.component.html',
  styleUrls: ['./batting-stats.component.scss']
})
export class BattingStatsComponent implements OnInit {
  @Input() year: number;
  @Input() inPO: boolean;
  @Input() league: number;
  batters: BattingStatModel[];
  columnDefs: any[];
  constructor(private statsService: StatsService) { }
  defaultColDef = {
    sortable: true
  };

  ngOnInit(): void {
    this.statsService.getBattingStats(this.league, this.year, this.inPO).subscribe( response => {
      this.batters = response;
      this.columnDefs = [
        {headerName: 'Player', cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: {route: '/players', year: this.year, idField: 'playerId',
        valueField: 'playerName' }}
        },
        {headerName: 'Team', cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: {route: '/teams', year: this.year, idField: 'teamId',
        valueField: 'teamName' }}
        },
        // {headerName: 'Org', cellRendererFramework: LinkRendererComponent,
        //   cellRendererParams: { inRouterLink: {route: '/teams', year: this.year, idField: ['team', 'organizationId'],
        //   valueField: ['team', 'organization'] }}
        // },
        {headerName: 'G', field: 'g' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'PA', field: 'pa' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'AB', field: 'ab' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'H', field: 'h' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: '1B', field: 'h1B' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: '2B', field: 'h2B' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: '3B', field: 'h3B' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'HR', field: 'hr' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'RBI', field: 'rbi' , width: 60, cellClass: 'text-right bat-cell'},
        {headerName: 'BB', field: 'bb' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'K', field: 'k' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'BA', field: 'ba' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'OBP', field: 'obp' , width: 66, cellClass: 'text-right bat-cell'},
        {headerName: 'SLG', field: 'slg' , width: 66, cellClass: 'text-right bat-cell'},
        {headerName: 'OPS', field: 'ops' , width: 66, cellClass: 'text-right bat-cell'},
        {headerName: 'XBH', field: 'xbh' , width: 66, cellClass: 'text-right bat-cell'},
        {headerName: 'BBK', field: 'bbk' , width: 66, cellClass: 'text-right bat-cell'},
        {headerName: 'ABHR', field: 'abhr' , width: 71, cellClass: 'text-right bat-cell'},
        {headerName: 'TB', field: 'tb' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'HBP', field: 'hbp' , width: 64, cellClass: 'text-right bat-cell'},
        {headerName: 'SH', field: 'sh' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'SF', field: 'sf' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'IBB', field: 'ibb' , width: 60, cellClass: 'text-right bat-cell'},
      ];
    });
  }

}
