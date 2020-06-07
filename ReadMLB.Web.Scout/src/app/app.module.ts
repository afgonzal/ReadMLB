import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AgGridModule} from 'ag-grid-angular';
import { HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { TeamsComponent } from './teams/teams.component';
import { BatterComponent } from './batter/batter.component';
import { PitcherComponent } from './pitcher/pitcher.component';
import { TeamDetailComponent } from './teams/team-detail/team-detail.component';
import { HeaderComponent } from './header/header.component';
import { RostersComponent } from './rosters/rosters.component';
import { RosterListComponent } from './rosters/roster-list/roster-list.component';
import { PlayersComponent } from './players/players.component';
import { PlayerDetailComponent } from './players/player-detail/player-detail.component';
import { PlayerLinkRendererComponent } from './player-link-renderer.component';
import { LeaguePipe } from './teams/league-pipe.pipe';
import { PlayerRunningComponent } from './players/player-running/player-running.component';
import { PlayerFieldRunningComponent } from './players/player-field-running/player-field-running.component';
import { PlayerDefenseComponent } from './players/player-defense/player-defense.component';
import { LeagueComponent } from './teams/league/league.component';
import { DivisionComponent } from './teams/division/division.component';
import { OrganizationComponent } from './teams/organization/organization.component';
import { PlayerSearchComponent } from './players/player-search/player-search.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LinkRendererComponent } from './link-renderer.component';
import { StatsComponent } from './stats/stats.component';
import { BattingStatsComponent } from './stats/batting-stats/batting-stats.component';
import { PitchingStatsComponent } from './stats/pitching-stats/pitching-stats.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    TeamsComponent,
    BatterComponent,
    PitcherComponent,
    TeamDetailComponent,
    HeaderComponent,
    RostersComponent,
    RosterListComponent,
    PlayersComponent,
    PlayerDetailComponent,
    PlayerLinkRendererComponent,
    LinkRendererComponent,
    LeaguePipe,
    PlayerRunningComponent,
    PlayerFieldRunningComponent,
    PlayerDefenseComponent,
    LeagueComponent,
    DivisionComponent,
    OrganizationComponent,
    PlayerSearchComponent,
    StatsComponent,
    BattingStatsComponent,
    PitchingStatsComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    AppRoutingModule,
    HttpClientModule,
    AgGridModule.withComponents(null)
  ],
  providers: [LeaguePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
