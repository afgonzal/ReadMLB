import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.scss']
})
export class TeamDetailComponent implements OnInit {

  constructor(private route: ActivatedRoute) { }
  teamId: number  = this.route.snapshot.params.id;
  team;

  ngOnInit(): void {
    console.log(this.teamId);
    this.team = {TeamCity: 'New York', TeamName: 'Yankees', TeamAbr: 'NYY', Organization: 'NYY'};
    this.team.Logo = this.team.Organization + '-' + this.team.TeamAbr;
  }

}
