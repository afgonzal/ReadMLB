import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.scss']
})
export class TeamDetailComponent implements OnInit {

  constructor() { }
  team;

  ngOnInit(): void {
    this.team = {TeamCity: 'New York', TeamName: 'Yankees', TeamAbr: 'NYY', Org: 'NYY'};
    this.team.Logo = 'assets/logos/' + this.team.Org + '-' + this.team.TeamAbr + '.png';
  }

}
