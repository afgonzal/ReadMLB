import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PlayerModel } from './player.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators'


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

  transformPlayer(apiPlayer: PlayerModel): PlayerModel{
    return Object.assign(new PlayerModel(), apiPlayer);
  }
}
