import { Component, OnInit } from '@angular/core';
import { PlayerService } from './players.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Params } from '@angular/router';
import { PlayerModel } from './player.model';

@Component({
  selector: 'app-players',
  templateUrl: './players.component.html',
  styleUrls: ['./players.component.scss']
})
export class PlayersComponent implements OnInit {
  active = 1;
  constructor(private playerService: PlayerService, private route: ActivatedRoute) { }
  player: PlayerModel = null;
  playerSubscription: Subscription;
  ngOnInit(): void {
    this.route.params.subscribe(
      (params: Params) => {
        this.playerSubscription = this.playerService.getPlayer(+params.id).subscribe(response  => {
          this.player = response;
        });
      }
    );

  }



}
