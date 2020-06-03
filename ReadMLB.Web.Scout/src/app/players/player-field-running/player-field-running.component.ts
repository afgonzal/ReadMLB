import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { PlayerService } from '../players.service';
import { LeaguePipe } from '../../teams/league-pipe.pipe';
import { Subscription } from 'rxjs';
import { FieldRunningStatModel } from '../player-running/runningStat.model';

@Component({
  selector: 'app-player-field-running',
  templateUrl: './player-field-running.component.html',
  styleUrls: ['./player-field-running.component.scss']
})
export class PlayerFieldRunningComponent implements OnInit, OnDestroy {
  @Input() inPO = false;
  playerSubscription: Subscription;
  routeSubscription: Subscription;
  playerStats: FieldRunningStatModel[];
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
    {headerName: 'PO', field: 'po' , width: 60, cellClass: 'text-right bat-cell'},
    {headerName: 'ASST', field: 'asst' , width: 68, cellClass: 'text-right bat-cell'},
    {headerName: 'ERR', field: 'err' , width: 65, cellClass: 'text-right bat-cell'},
    {headerName: 'FLD', field: 'fld' , width: 65, cellClass: 'text-right bat-cell'},
    {headerName: 'Ch', field: 'ch' , width: 65, cellClass: 'text-right bat-cell'},
  ];


  ngOnInit(): void {
    this.routeSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.playerSubscription = this.playersService.getFieldRunningStats(+params.id, this.inPO).subscribe( response  => {
          this.playerStats = response;
        });
      }
    );
  }

  ngOnDestroy(): void {
    this.routeSubscription.unsubscribe();
    this.playerSubscription.unsubscribe();
  }

}
