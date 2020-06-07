import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DivisionModel, TeamModel } from './team.model';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { PlayerModel } from '../players/player.model';

@Injectable({providedIn: 'root'})
export class TeamsService {
  constructor(private http: HttpClient) {  }

  getLeagueDivisions(league: number): Observable<DivisionModel[]> {
    return this.http.get<DivisionModel[]>(environment.API_URL + 'teams/league/' + league + '/divisions');
  }

  getTeam(teamId: number): Observable<TeamModel> {
    return  this.http.get<TeamModel>(environment.API_URL + 'teams/' + teamId);
  }

  getOrganization(organizationId: number): Observable<TeamModel[]> {
    return this.http.get<TeamModel[]>(environment.API_URL + 'teams/organization/' + organizationId);
  }

  getLeagueTeams(league: number): Observable<TeamModel[]> {
    return this.http.get<TeamModel[]>(environment.API_URL + 'teams/league/' + league);
  }

  getRoster(teamId: number, year: number): Observable<PlayerModel[]> {
    return this.http.get<PlayerModel[]>(environment.API_URL + 'roster/' + teamId + '/' + year + '/false');
  }
}
