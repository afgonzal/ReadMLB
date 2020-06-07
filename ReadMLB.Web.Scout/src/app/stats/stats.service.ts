import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BattingStatModel } from '../batter/battingStat.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { PitchingStatModel } from '../pitcher/pitchingStat.model';

@Injectable({providedIn: 'root'})
export class StatsService {

  constructor(private http: HttpClient) {}

  getBattingStats(league: number, year: number, inPO: boolean, teamId?: number): Observable<BattingStatModel[]> {
    let params = new HttpParams().set('inPO', inPO.toString());
    if (teamId) {
      params = params.set('teamId', teamId.toString());
    }
    return this.http.get<BattingStatModel[]>(environment.API_URL + 'batting/league/' + league + '/' + year, {
      params
    }).pipe(map( stats => {
      return stats.map( stat => {
        return this.transformBattingStat(stat);
      });
    }));
  }

  getPitchingStats(league: number, year: number, inPO: boolean, teamId?: number): Observable<PitchingStatModel[]> {
    let params = new HttpParams().set('inPO', inPO.toString());
    if (teamId) {
      params = params.set('teamId', teamId.toString());
    }
    return this.http.get<PitchingStatModel[]>(environment.API_URL + 'pitching/league/' + league + '/' + year, {
      params
    });
  }

  transformBattingStat(stat: BattingStatModel): BattingStatModel {
    return Object.assign(new BattingStatModel(), stat);
  }
}
