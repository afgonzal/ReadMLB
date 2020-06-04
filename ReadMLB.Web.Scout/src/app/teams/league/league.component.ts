import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Params } from '@angular/router';
import { TeamsService } from '../teams.service';
import { DivisionModel } from '../team.model';

@Component({
  selector: 'app-league',
  templateUrl: './league.component.html',
  styleUrls: ['./league.component.scss']
})
export class LeagueComponent implements OnInit, OnDestroy {
  paramsObserver: Subscription;
  leagueSubscription: Subscription;
  league: number;
  divisions: DivisionModel[];
  constructor(private route: ActivatedRoute, private teamsService: TeamsService) { }

  ngOnInit(): void {
    this.paramsObserver = this.route.params.subscribe(
      (params: Params) => {
        this.league =  + this.route.snapshot.params.id;
        this.leagueSubscription = this.teamsService.getLeagueDivisions(this.league).subscribe(response => {
          this.divisions = response;
      });
    });
  }

  ngOnDestroy(): void {
    this.paramsObserver.unsubscribe();
    this.leagueSubscription.unsubscribe();
  }

}
