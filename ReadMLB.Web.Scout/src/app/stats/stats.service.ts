import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BattingStatModel } from '../batter/battingStat.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({providedIn: 'root'})
export class StatsService {

  constructor(private http: HttpClient) {}

  getBattingStats(league: number, year: number, inPO: boolean): Observable<BattingStatModel[]> {
    return this.http.get<BattingStatModel[]>(environment.API_URL + 'batting/league/' + league + '/' + year, {
      params: new HttpParams().set('inPO', inPO.toString())
    }).pipe(map( stats => {
      return stats.map( stat => {
        return this.transformBattingStat(stat);
      });
    }));
  }

  transformBattingStat(stat: BattingStatModel): BattingStatModel {
    return Object.assign(new BattingStatModel(), stat);
  }
}
