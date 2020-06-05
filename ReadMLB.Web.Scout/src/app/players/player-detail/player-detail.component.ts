import { Subscription } from 'rxjs';
import { ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { PlayerService } from '../players.service';
import { PlayerModel } from '../player.model';

@Component({
  selector: 'app-player-detail',
  templateUrl: './player-detail.component.html',
  styleUrls: ['./player-detail.component.scss']
})
export class PlayerDetailComponent implements OnInit {
  active = 1;
  year: number;
  inPO: boolean;
  constructor(private playerService: PlayerService, private route: ActivatedRoute) { }
  player: PlayerModel = null;
  playerSubscription: Subscription;
  ngOnInit(): void {
    this.route.params.subscribe(
      (params: Params) => {
        this.year = this.route.snapshot.queryParams.year;
        this.inPO = false;
        this.playerSubscription = this.playerService.getPlayer(+params.id, this.inPO).subscribe(response  => {
          this.player = response;
        });
      }
    );

  }



}
