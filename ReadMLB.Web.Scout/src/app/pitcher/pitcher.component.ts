import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { PitchingStatModel } from './pitchingStat.model';
import { LeaguePipe } from '../teams/league-pipe.pipe';
import { PlayerService } from '../players/players.service';
import { ActivatedRoute, Params } from '@angular/router';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-pitcher',
  templateUrl: './pitcher.component.html',
  styleUrls: ['./pitcher.component.scss']
})
export class PitcherComponent implements OnInit, OnDestroy {
  @Input() inPO: boolean;
  playerSubscription: Subscription;
  routeSubscription: Subscription;
  pitchingStats: PitchingStatModel[];
  rowStyle = 'bat-row';
  columnDefs = [
    {headerName: 'Year', field: 'year', width: 62, cellClass: 'text-right bat-cell', tooltipField: 'organization'},
    {headerName: 'Tm', field: 'teamAbr', width: 57, cellClass: 'bat-cell', tooltipField: 'teamName'},
    {headerName: 'Lg', valueFormatter(params) {
      const pipe = new LeaguePipe();
      return pipe.transform(params.data.league);
    } , width: 60, cellClass: 'bat-cell'},
    {headerName: 'W', field: 'w' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'L', field: 'l' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'W-L%', width: 71, cellClass: 'text-right bat-cell', valueFormatter(params) {
      const pipe = new DecimalPipe('en');
      return pipe.transform(params.data.wp, '2.0-0');
    }},
    {headerName: 'ERA', field: 'era' , width: 63, cellClass: 'text-right bat-cell'},
    {headerName: 'G', field: 'g' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'GS', field: 'gs' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'CG', field: 'cg' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'SHO', field: 'sho' , width: 63, cellClass: 'text-right bat-cell'},
    {headerName: 'SV', field: 'sv' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'BSV', field: 'bsv' , width: 63, cellClass: 'text-right bat-cell'},
    {headerName: 'IP', width: 57, cellClass: 'text-right bat-cell', valueFormatter(params) {
      return params.data.iP10 / 10;
    }},
    {headerName: 'IPG', field: 'ipg' , width: 63, cellClass: 'text-right bat-cell'},
    {headerName: 'H', field: 'h' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'R', field: 'r' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'ER', field: 'er' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: '1B', field: 'h1B' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: '2B', field: 'h2B' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: '3B', field: 'h3B' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'HR', field: 'hr' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'BB', field: 'bb' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'IBB', field: 'ibb' , width: 63, cellClass: 'text-right bat-cell'},
    {headerName: 'SO', field: 'k' , width: 57, cellClass: 'text-right bat-cell'},
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

  constructor(private playerService: PlayerService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.routeSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.playerSubscription = this.playerService.getPitchingStats(+params.id, this.inPO).subscribe(response  => {
          this.pitchingStats = response;
        });
      }
    );
  }

  ngOnDestroy(): void {
    this.routeSubscription.unsubscribe();
    this.playerSubscription.unsubscribe();
  }

}
