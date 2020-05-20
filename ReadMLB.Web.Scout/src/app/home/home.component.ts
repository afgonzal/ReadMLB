import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  currentOrganization = {
    teams: [],
    year: 2008,
    inPO: false
    };

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<any>(environment.API_URL + 'settings').subscribe(settings => {
      this.currentOrganization.teams = settings.teams;
      this.currentOrganization.year = settings.year;
      this.currentOrganization.inPO = settings.inPO;
    });
  }

  getLogo(league: number): string {
    if (this.currentOrganization.teams[league].teamAbr === 'N/A'){
      return 'milb';
    }
    return this.currentOrganization.teams[league].organization + '-' + this.currentOrganization.teams[league].teamAbr.toUpperCase();
  }
}
