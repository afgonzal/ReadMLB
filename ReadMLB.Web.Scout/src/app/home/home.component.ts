import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { SessionService } from '../session.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {
  currentOrganization = {
    teams: [],
    year: 2008,
    inPO: false
    };
    sessionSubscription: Subscription;

  constructor(private session: SessionService) { }

  ngOnInit() {
    this.sessionSubscription = this.session.settingsObserver.subscribe( settings => {
      this.currentOrganization.teams = settings.organization;
      this.currentOrganization.year = settings.currentYear;
      this.currentOrganization.inPO = settings.inPO;
      localStorage.setItem('currentYear', settings.currentYear.toString());
      localStorage.setItem('organization', JSON.stringify(settings.organization));
    });
  }

  getLogo(league: number): string {
    if (this.currentOrganization.teams[league].teamAbr === 'N/A'){
      return 'milb';
    }
    return this.currentOrganization.teams[league].organization + '-' + this.currentOrganization.teams[league].teamAbr.toUpperCase();
  }

  ngOnDestroy(): void {
    this.sessionSubscription.unsubscribe();

  }
}
