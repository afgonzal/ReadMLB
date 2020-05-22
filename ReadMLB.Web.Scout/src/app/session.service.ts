import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { SettingsModel } from './settings.model';

@Injectable({providedIn: 'root'})
export class SessionService {
  currentYear: number;
  currentOrganization: [];
  inPO = false;
  public settingsObserver: Observable<SettingsModel>;
  constructor(private http: HttpClient) {
    this.settingsObserver = new Observable<SettingsModel>( observer => {
    this.http.get<any>(environment.API_URL + 'settings').subscribe(settings => {
        this.currentOrganization = settings.teams;
        this.currentYear = settings.year;
        this.inPO = settings.inPO;
        observer.next({organization: this.currentOrganization, currentYear: this.currentYear, inPO: this.inPO});
      });
    });
  }
}
