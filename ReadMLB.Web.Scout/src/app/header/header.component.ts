import { Component, OnInit } from '@angular/core';
import { SessionService } from '../session.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  currentYear: number;
  organizationName: string;
  constructor(private session: SessionService) { }
  sessionSubscription: Subscription;
  ngOnInit(): void {
    this.sessionSubscription = this.session.settingsObserver.subscribe( settings => {
      this.currentYear = settings.currentYear;
      this.organizationName = settings.organization[0].teamAbr;
    });
  }

}
