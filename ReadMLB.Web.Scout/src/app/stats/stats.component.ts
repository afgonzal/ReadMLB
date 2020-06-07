import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { TeamsService } from '../teams/teams.service';
import { TeamModel } from '../teams/team.model';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.scss']
})
export class StatsComponent implements OnInit {
  filterForm: FormGroup;
  leagues = [0, 1, 2];
  years = [2008, 2009];
  teams: TeamModel[];
  constructor(private teamsService: TeamsService) { }

  ngOnInit(): void {
    this.filterForm = new FormGroup({
      league: new FormControl(0),
      year: new FormControl(+localStorage.getItem('currentYear')),
      team: new FormControl(null)
    });
    this.filterForm.get('league').valueChanges.subscribe(event => this.leagueChanged(event));
    this.leagueChanged(null);
  }

  leagueChanged(event) {
    this.teamsService.getLeagueTeams(this.filterForm.get('league').value).subscribe(response => {
      this.teams = response;
    });
  }

  onFilter() {

  }
}
