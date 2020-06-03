import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { PlayerService } from '../players.service';
import { LeaguePipe } from 'src/app/teams/league-pipe.pipe';
import { Subscription } from 'rxjs';
import { RunningStatModel } from './runningStat.model';

@Component({
  selector: 'app-player-running',
  templateUrl: './player-running.component.html',
  styleUrls: ['./player-running.component.scss']
})
export class PlayerRunningComponent implements OnInit {
  @Input() inPO: boolean;
  playerSubscription: Subscription;
  routeSubscription: Subscription;
  runningStats: RunningStatModel[];
  constructor(private route: ActivatedRoute, private playersService: PlayerService) { }

  rowStyle = 'bat-row';
  columnDefs = [
    {headerName: 'Year', field: 'year', width: 62, cellClass: 'text-right bat-cell', tooltipField: 'organization'},
    {headerName: 'Tm', field: 'teamAbr', width: 57, cellClass: 'bat-cell', tooltipField: 'teamName'},
    {headerName: 'Lg', valueFormatter(params) {
      const pipe = new LeaguePipe();
      return pipe.transform(params.data.league);
    } , width: 60, cellClass: 'bat-cell'},
    {headerName: 'RS', field: 'rs' , width: 60, cellClass: 'text-right bat-cell'},
    {headerName: 'SB', field: 'sb' , width: 60, cellClass: 'text-right bat-cell'},
    {headerName: 'CS', field: 'cs' , width: 60, cellClass: 'text-right bat-cell'},
  ];


  ngOnInit(): void {
    this.routeSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.playerSubscription = this.playersService.getRunningStats(+params.id, this.inPO).subscribe( response  => {
          this.runningStats = response;
        });
      }
    );
  }

  ngOnDestroy(): void {
    this.routeSubscription.unsubscribe();
    this.playerSubscription.unsubscribe();
  }
}
