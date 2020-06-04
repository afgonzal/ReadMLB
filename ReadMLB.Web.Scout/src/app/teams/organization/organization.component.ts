import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { TeamsService } from '../teams.service';
import { TeamModel } from '../team.model';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss']
})
export class OrganizationComponent implements OnInit, OnDestroy {
  @Input() organizationId: number;
  @Input() year: number;
  @Input() selectedTeamId: number;
  organizationSubscription: Subscription;
  teams: TeamModel[];
  constructor(private teamsService: TeamsService) { }

  ngOnInit(): void {
    this.organizationSubscription = this.teamsService.getOrganization(this.organizationId).subscribe( response => {
      this.teams = response;
    });
  }

  ngOnDestroy(): void {

  }

}
