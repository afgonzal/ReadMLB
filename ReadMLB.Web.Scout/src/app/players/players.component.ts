import { Component, OnInit } from '@angular/core';
import { PlayerService } from './players.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-players',
  templateUrl: './players.component.html',
  styleUrls: ['./players.component.scss']
})
export class PlayersComponent implements OnInit {
  constructor(private playerService: PlayerService, private route: ActivatedRoute) { }
  ngOnInit(): void {

  }

}
