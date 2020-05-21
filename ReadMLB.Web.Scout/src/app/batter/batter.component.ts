import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { PlayerService } from '../players/players.service';
import { BattingStatModel } from './battingStat.model';
import { LeaguePipe } from '../teams/league-pipe.pipe';

@Component({
  selector: 'app-batter',
  templateUrl: './batter.component.html',
  styleUrls: ['./batter.component.scss'],
  providers: [LeaguePipe]
})
export class BatterComponent implements OnInit {
  @Input() inPO: boolean;
  playerSubscription: Subscription;
  battingStats: BattingStatModel[];
  rowStyle = 'bat-row';
  columnDefs = [
    {headerName: 'Year', field: 'year', width: 58, cellClass: 'text-right bat-cell'},
    {headerName: 'Tm', field: 'teamAbr', width: 57, cellClass: 'bat-cell'},
    {headerName: 'Lg', valueFormatter(params) {
      const pipe = new LeaguePipe();
      return pipe.transform(params.data.league);
    } , width: 60, cellClass: 'bat-cell'},
    {headerName: 'G', field: 'g' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'PA', field: 'pa' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'AB', field: 'ab' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'H', field: 'h' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: '1B', field: 'h1B' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: '2B', field: 'h2B' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: '3B', field: 'h3B' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'HR', field: 'hr' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'RBI', field: 'rbi' , width: 60, cellClass: 'text-right bat-cell'},
    {headerName: 'BB', field: 'bb' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'SO', field: 'so' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'BA', field: 'ba' , width: 60, cellClass: 'text-right bat-cell'},
    {headerName: 'OBP', field: 'obp' , width: 63, cellClass: 'text-right bat-cell'},
    {headerName: 'SLG', field: 'slg' , width: 60, cellClass: 'text-right bat-cell'},
    {headerName: 'OPS', field: 'ops' , width: 63, cellClass: 'text-right bat-cell'},
    {headerName: 'XBH', field: 'xbh' , width: 60, cellClass: 'text-right bat-cell'},
    {headerName: 'BBK', field: 'bbk' , width: 63, cellClass: 'text-right bat-cell'},
    {headerName: 'ABHR', field: 'abhr' , width: 68, cellClass: 'text-right bat-cell'},
    {headerName: 'TB', field: 'tb' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'HBP', field: 'hbp' , width: 60, cellClass: 'text-right bat-cell'},
    {headerName: 'SH', field: 'sh' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'SF', field: 'sf' , width: 57, cellClass: 'text-right bat-cell'},
    {headerName: 'IBB', field: 'ibb' , width: 60, cellClass: 'text-right bat-cell'},

  ];


  constructor(private playerService: PlayerService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(
      (params: Params) => {
        this.playerSubscription = this.playerService.getBattingStats(+params.id, false).subscribe(response  => {
          this.battingStats = response.filter(bs => bs.battingVs === 'Total');
        });
      }
    );
    }
  }
