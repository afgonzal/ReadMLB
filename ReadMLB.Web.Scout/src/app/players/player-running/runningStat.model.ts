export class RunningStatModel {
  public year: number;
  public league: number;
  public teamId: number;
  public teamName: string;
  public inPO: boolean;
  public rs: number;
  public sb: number;
  public cs: number;
}

export class DefenseStatModel {
  public year: number;
  public league: number;
  public teamId: number;
  public teamName: string;
  public inPO: boolean;
  public asst: number;
  public po: number;
  public err: number;
  public fld: number;
}

export class FieldRunningStatModel {
  public year: number;
  public league: number;
  public teamId: number;
  public teamName: string;
  public inPO: boolean;
  public asst: number;
  public po: number;
  public err: number;
  public fld: number;
  public rs: number;
  public sb: number;
  public cs: number;
}
