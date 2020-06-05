import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PlayerModel } from './player.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators'
import { BattingStatModel } from '../batter/battingStat.model';
import { RunningStatModel, FieldRunningStatModel } from './player-running/runningStat.model';
import { PitchingStatModel } from '../pitcher/pitchingStat.model';


@Injectable({providedIn: 'root'})
export class PlayerService {
  constructor(private http: HttpClient) {  }

  getPlayer(playerId: number, year: number, inPO: boolean): Observable<PlayerModel> {
    if (!year) {
      year = +localStorage.getItem('currentYear');
    }
    return this.http.get<PlayerModel>(environment.API_URL + 'players/' + playerId, {
     params: new HttpParams().set('year', year.toString()).set('inPO', inPO.toString())
   }).pipe(map( (response: PlayerModel) => {
     return this.transformPlayer(response);
   }));
  }

  getBattingStats(playerId: number, inPO: boolean): Observable<BattingStatModel[]> {
    return this.http.get<BattingStatModel[]>(environment.API_URL + 'batting/' + playerId, {
      params: new HttpParams().set('inPO', inPO.toString())
    }).pipe(map( stats => {
      return stats.map( stat => {
        return this.transformBattingStat(stat);
      });
    }));
  }

  getRunningStats(playerId: number, inPO: boolean): Observable<RunningStatModel[]> {
    return this.http.get<RunningStatModel[]>(environment.API_URL + 'running/' + playerId, {
      params: new HttpParams().set('inPO', inPO.toString())
    });
  }

  getDefenseStats(playerId: number, inPO: boolean): Observable<RunningStatModel[]> {
    return this.http.get<RunningStatModel[]>(environment.API_URL + 'defense/' + playerId, {
      params: new HttpParams().set('inPO', inPO.toString())
    });
  }

  getFieldRunningStats(playerId: number, inPO: boolean): Observable<FieldRunningStatModel[]> {
    return this.http.get<FieldRunningStatModel[]>(environment.API_URL + 'players/' + playerId + '/fieldRunning', {
      params: new HttpParams().set('inPO', inPO.toString())
    });
  }

  getPitchingStats(playerId: number, inPO: boolean): Observable<PitchingStatModel[]> {
    return this.http.get<PitchingStatModel[]>(environment.API_URL + 'pitching/' + playerId, {
      params: new HttpParams().set('inPO', inPO.toString())
    });
  }

  transformPlayer(apiPlayer: PlayerModel): PlayerModel {
    return Object.assign(new PlayerModel(), apiPlayer);
  }

  transformBattingStat(stat: BattingStatModel): BattingStatModel {
    return Object.assign(new BattingStatModel(), stat);
  }

  searchPlayers(firstName: string, lastName: string, league?: number ){
    return this.http.post<PlayerModel[]>(environment.API_URL + 'players/search',
    {league, firstName, lastName});
  }


}
