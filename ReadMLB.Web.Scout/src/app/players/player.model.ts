import { TeamModel } from '../teams/team.model';

export class PlayerModel {
  public firstName: string;
  public lastName: string;
  public shirt: number;
  public primaryPosition: string;
  public secondaryPosition: string;
  public bats: string;
  public throws: string;
  public team: TeamModel;
  public teamHistory?: PlayerTeamHistoryModel[];

  public position(): string {
    let retVal = this.primaryPosition;
    if (this.secondaryPosition && this.primaryPosition !== this.secondaryPosition) {
      retVal += ' / ' + this.secondaryPosition;
    }
    return retVal;
  }
  public fullName(): string {
    return this.firstName + ' ' + this.lastName;
  }

}

export interface PlayerTeamHistoryModel {
  teamId: number;
  teamAbr: string;
  teamName: string;
  organizationId: number;
  organization: string;
  league: number;
  year: number;
  inPO: boolean;
  slot: number;
}
