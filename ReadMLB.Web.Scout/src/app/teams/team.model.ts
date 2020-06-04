export interface TeamModel {
  league: number;
  division: number;
  teamName: string;
  teamAbr: string;
  cityName: string;
  teamId: number;
  organizationId: number;
  organization: string;
  logo: string;
}

export interface DivisionModel {
  league: number;
  divisionId: number;
  name: string;
  teams: TeamModel[];
}
