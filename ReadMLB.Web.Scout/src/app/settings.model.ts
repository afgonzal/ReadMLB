import { TeamModel } from './teams/team.model';

export interface SettingsModel {
  currentYear: number;
  organization: TeamModel[];
  inPO: boolean;
}

