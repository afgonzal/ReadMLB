import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { TeamsComponent } from './teams/teams.component';
import { BatterComponent } from './batter/batter.component';
import { PitcherComponent } from './pitcher/pitcher.component';
import { TeamDetailComponent } from './teams/team-detail/team-detail.component';
import { HeaderComponent } from './header/header.component';
import { RostersComponent } from './rosters/rosters.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    TeamsComponent,
    BatterComponent,
    PitcherComponent,
    TeamDetailComponent,
    HeaderComponent,
    RostersComponent
  ],
  imports: [
    BrowserModule,
    NgbModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
