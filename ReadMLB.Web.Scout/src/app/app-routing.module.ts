import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TeamsComponent } from './teams/teams.component';
import { TeamDetailComponent } from './teams/team-detail/team-detail.component';
import { RostersComponent } from './rosters/rosters.component';
import { PlayersComponent } from './players/players.component';
import { PlayerDetailComponent } from './players/player-detail/player-detail.component';
import { BatterComponent } from './batter/batter.component';
import { LeagueComponent } from './teams/league/league.component';
import { PlayerSearchComponent } from './players/player-search/player-search.component';


const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'teams', component: TeamsComponent, children: [
    {path: 'league/:id', component: LeagueComponent},
    {path: ':id', component: TeamDetailComponent }
  ]},
  {path: 'rosters', component: RostersComponent},
  {path: 'players', component: PlayersComponent, children: [
    {path: 'search', component: PlayerSearchComponent},
    {path: ':id', component: PlayerDetailComponent}
  ]},
  {path: 'batting/:id', component: BatterComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
