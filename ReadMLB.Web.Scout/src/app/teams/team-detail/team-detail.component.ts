import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { TeamModel } from '../team.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.scss']
})
export class TeamDetailComponent implements OnInit, OnDestroy {

  constructor(private route: ActivatedRoute, private http: HttpClient) { }
  teamId: number;
  team: TeamModel;
  year: number;
  organizationLogo: string;
  paramsObserver: Subscription;

  ngOnInit(): void {
    this.paramsObserver = this.route.params.subscribe(
      (params: Params) => {
        this.teamId =  this.route.snapshot.params.id;
        this.year = this.route.snapshot.queryParams.year;

        this.http.get<TeamModel>(environment.API_URL + 'teams/' + this.teamId).subscribe(response => {
        this.team = response;
        this.team.logo = this.team.organization + '-' + this.team.teamAbr.toUpperCase();
        this.organizationLogo = this.team.organization + '-' + this.team.organization;
      });
    });
  }

  ngOnDestroy(): void {
    this.paramsObserver.unsubscribe();
  }

}
