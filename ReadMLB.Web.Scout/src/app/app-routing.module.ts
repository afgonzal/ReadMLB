import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TeamsComponent } from './teams/teams.component';
import { TeamDetailComponent } from './teams/team-detail/team-detail.component';
import { RostersComponent } from './rosters/rosters.component';


const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'teams', component: TeamsComponent, children: [
    {path: ':id', component: TeamDetailComponent }
  ]},
  {path: 'rosters', component: RostersComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
