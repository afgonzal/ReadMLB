import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { PitchingStatModel } from 'src/app/pitcher/pitchingStat.model';
import { StatsService } from '../stats.service';
import { LeaguePipe } from 'src/app/teams/league-pipe.pipe';
import { DecimalPipe } from '@angular/common';
import { LinkRendererComponent } from 'src/app/link-renderer.component';

@Component({
  selector: 'app-pitching-stats',
  templateUrl: './pitching-stats.component.html',
  styleUrls: ['./pitching-stats.component.scss']
})
export class PitchingStatsComponent implements OnInit, OnChanges {
  @Input() year: number;
  @Input() inPO: boolean;
  @Input() league: number;
  @Input() teamId?: number;
  pitchers: PitchingStatModel[];
  columnDefs: any[];
  constructor(private statsService: StatsService) { }
  defaultColDef = {
    sortable: true
  };

  ngOnInit(): void {
  }

  ngOnChanges() {
    this.loadValues();
  }


  loadValues() {
    this.statsService.getPitchingStats(this.league, this.year, this.inPO, this.teamId).subscribe( response => {
      this.pitchers = response;
      this.columnDefs = [
        {headerName: 'Player', cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: {route: '/players', year: this.year, idField: 'playerId',
        valueField: 'playerName' }}
        },
        {headerName: 'Team', cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: {route: '/teams', year: this.year, idField: 'teamId',
        valueField: 'teamName' }}
        },
        {headerName: 'W', field: 'w' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'L', field: 'l' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'W-L%', width: 74, cellClass: 'text-right bat-cell', valueFormatter(params) {
          const pipe = new DecimalPipe('en');
          return pipe.transform(params.data.wp, '2.0-0');
        }},
        {headerName: 'ERA', field: 'era' , width: 71, cellClass: 'text-right bat-cell'},
        {headerName: 'G', field: 'g' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'GS', field: 'gs' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'CG', field: 'cg' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'SHO', field: 'sho' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'SV', field: 'sv' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'BSV', field: 'bsv' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'IP', width: 71, cellClass: 'text-right bat-cell', valueFormatter(params) {
          return params.data.iP10 / 10;
        }},
        {headerName: 'IPG', field: 'ipg' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'H', field: 'h' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'R', field: 'r' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'ER', field: 'er' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: '1B', field: 'h1B' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: '2B', field: 'h2B' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: '3B', field: 'h3B' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'HR', field: 'hr' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'BB', field: 'bb' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'IBB', field: 'ibb' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'SO', field: 'k' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'HBP', field: 'hb' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'BF', field: 'tpa' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'WHIP', field: 'whip' , width: 70, cellClass: 'text-right bat-cell'},
        {headerName: 'H9', field: 'h9' , width: 64, cellClass: 'text-right bat-cell'},
        {headerName: 'HR9', field: 'hR9' , width: 64, cellClass: 'text-right bat-cell'},
        {headerName: 'BB9', field: 'bB9' , width: 64, cellClass: 'text-right bat-cell'},
        {headerName: 'SO9', field: 'k9' , width: 64, cellClass: 'text-right bat-cell'},
        {headerName: 'KBB', field: 'kbb' , width: 64, cellClass: 'text-right bat-cell'},
        {headerName: 'PK', field: 'pk' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'SF', field: 'sf' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'SH', field: 'sh' , width: 57, cellClass: 'text-right bat-cell'}
      ];
    });
  }
}
