import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PlayerModel } from './player.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators'
import { BattingStatModel } from '../batter/battingStat.model';


@Injectable({providedIn: 'root'})
export class PlayerService {
  constructor(private http: HttpClient) {  }

  getPlayer(playerId: number): Observable<PlayerModel> {
    const year = localStorage.getItem('currentYear');
    return this.http.get<PlayerModel>(environment.API_URL + 'players/' + playerId, {
     params: new HttpParams().set('year', year).set('inPO', 'false')
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

  transformPlayer(apiPlayer: PlayerModel): PlayerModel {
    return Object.assign(new PlayerModel(), apiPlayer);
  }

  transformBattingStat(stat: BattingStatModel): BattingStatModel {
    return Object.assign(new BattingStatModel(), stat);
  }


}
