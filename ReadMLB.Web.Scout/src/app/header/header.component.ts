import { Component, OnInit, OnDestroy } from '@angular/core';
import { SessionService } from '../session.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  currentYear: number;
  organizationName: string;
  constructor(private session: SessionService) { }
  sessionSubscription: Subscription;
  ngOnInit(): void {
    this.sessionSubscription = this.session.settingsObserver.subscribe( settings => {
      this.currentYear = settings.currentYear;
      localStorage.setItem('currentYear', settings.currentYear.toString());
      this.organizationName = settings.organization[0].teamAbr;
    });
  }
  ngOnDestroy(): void {
    this.sessionSubscription.unsubscribe();
  }
}
