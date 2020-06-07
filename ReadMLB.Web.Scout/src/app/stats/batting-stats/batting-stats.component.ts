import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { BattingStatModel } from 'src/app/batter/battingStat.model';
import { StatsService } from '../stats.service';
import { LinkRendererComponent } from 'src/app/link-renderer.component';
import { Observable } from 'rxjs';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-batting-stats',
  templateUrl: './batting-stats.component.html',
  styleUrls: ['./batting-stats.component.scss']
})
export class BattingStatsComponent implements OnInit, OnChanges {
  @Input() year: number;
  @Input() inPO: boolean;
  @Input() league: number;
  @Input() teamId?: number;
  batters: BattingStatModel[];
  columnDefs: any[];
  constructor(private statsService: StatsService) { }
  defaultColDef = {
    sortable: true
  };

  ngOnInit(): void {
  }

  ngOnChanges() {
    this.loadStats();
  }

  loadStats(): void {
    this.statsService.getBattingStats(this.league, this.year, this.inPO, this.teamId).subscribe( response => {
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
        {headerName: 'Org', cellRendererFramework: LinkRendererComponent,
          cellRendererParams: { inRouterLink: {route: '/teams', year: this.year, idField: 'organizationId',
          valueField: 'organization' }}, width: 75
        },
        {headerName: 'G', field: 'g' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'PA', field: 'pa' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'AB', field: 'ab' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: 'H', field: 'h' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: '1B', field: 'h1B' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: '2B', field: 'h2B' , width: 63, cellClass: 'text-right bat-cell'},
        {headerName: '3B', field: 'h3B' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'HR', field: 'hr' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'RBI', field: 'rbi' , width: 60, cellClass: 'text-right bat-cell'},
        {headerName: 'BB', field: 'bb' , width: 60, cellClass: 'text-right bat-cell'},
        {headerName: 'K', field: 'k' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'BA', field: 'ba' , width: 70, cellClass: 'text-right bat-cell'},
        {headerName: 'OBP', field: 'obp' , width: 70, cellClass: 'text-right bat-cell'},
        {headerName: 'SLG', field: 'slg' , width: 70, cellClass: 'text-right bat-cell'},
        {headerName: 'OPS', field: 'ops' , width: 70, cellClass: 'text-right bat-cell'},
        {headerName: 'XBH', field: 'xbh' , width: 66, cellClass: 'text-right bat-cell'},
        {headerName: 'BBK', width: 77, cellClass: 'text-right bat-cell', valueFormatter(params) {
          const pipe = new DecimalPipe('en');
          return pipe.transform(params.data.bbk, '1.4-4');
        }},
        {headerName: 'ABHR', width: 70, cellClass: 'text-right bat-cell', valueFormatter(params) {
          const pipe = new DecimalPipe('en');
          return pipe.transform(params.data.abhr, '2.0-0');
        }},
        {headerName: 'TB', field: 'tb' , width: 64, cellClass: 'text-right bat-cell'},
        {headerName: 'HBP', field: 'hbp' , width: 64, cellClass: 'text-right bat-cell'},
        {headerName: 'SH', field: 'sh' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'SF', field: 'sf' , width: 57, cellClass: 'text-right bat-cell'},
        {headerName: 'IBB', field: 'ibb' , width: 60, cellClass: 'text-right bat-cell'},
      ];
    });
  }

  decimalComparator(d1, d2, d3, d4, isInverted) {
    return 0;
  }
}
