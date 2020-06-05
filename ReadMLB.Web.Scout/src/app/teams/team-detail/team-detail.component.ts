import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { TeamModel } from '../team.model';
import { Subscription } from 'rxjs';
import { TeamsService } from '../teams.service';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.scss']
})
export class TeamDetailComponent implements OnInit, OnDestroy {

  constructor(private route: ActivatedRoute, private teamsService: TeamsService) { }
  teamId: number;
  team: TeamModel;
  year: number;
  organizationLogo: string;
  paramsObserver: Subscription;
  teamSubscription: Subscription;

  ngOnInit(): void {
    this.paramsObserver = this.route.params.subscribe(
      (params: Params) => {
        this.teamId =  this.route.snapshot.params.id;
        this.year = +this.route.snapshot.queryParams.year;
        if (!this.year) {
          this.year = +localStorage.getItem('currentYear');
        }

        this.teamSubscription = this.teamsService.getTeam(this.teamId).subscribe(response => {
          this.team = response;
          this.team.logo = this.team.organization.toUpperCase() + '-' + this.team.teamAbr.toUpperCase();
          this.organizationLogo = this.team.organization.toUpperCase() + '-' + this.team.organization.toUpperCase();
        });
    });
  }

  ngOnDestroy(): void {
    this.paramsObserver.unsubscribe();
    this.teamSubscription.unsubscribe();
  }

}
