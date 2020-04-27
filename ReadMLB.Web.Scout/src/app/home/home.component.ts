import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  currentOrganization = {
    teams: [
      { teamId : 64,
        teamAbr: 'NYY',
        organization : 'NYY'
      },
      {
        teamId: 37,
        teamAbr: 'SWB',
        organization : 'NYY'
      },
      {
        teamId: 90,
        teamAbr: 'Tre',
        organization : 'NYY'
      }]
    };

  constructor() { }

  ngOnInit() {
  }

  getLogo(league: number): string {
    return this.currentOrganization.teams[league].organization + '-' + this.currentOrganization.teams[league].teamAbr.toUpperCase();
  }
}
