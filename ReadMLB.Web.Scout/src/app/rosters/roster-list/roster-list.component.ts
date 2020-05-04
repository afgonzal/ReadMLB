import { Component, OnInit } from '@angular/core';
import { PlayerLinkRendererComponent } from 'src/app/player-link-renderer.component';

@Component({
  selector: 'app-roster-list',
  templateUrl: './roster-list.component.html',
  styleUrls: ['./roster-list.component.scss']
})
export class RosterListComponent implements OnInit {
  columnDefs = [
    {headerName: 'Player', cellRendererFramework: PlayerLinkRendererComponent,
    cellRendererParams: { inRouterLink: '/players' }
    },
    {headerName: 'Position', valueGetter: function(params){
      let retVal: string = params.data.primaryPosition;
      if (params.data.secondaryPosition && params.data.primaryPosition != params.data.secondaryPosition) {
        retVal += ' / ' + params.data.secondaryPosition;
      }
      return retVal;
    }},
    {headerName: '#', field: 'shirt'},
    {headerName: 'B/T', valueGetter: function(params){
      return params.data.bats + '/' + params.data.throws;
    }}
  ];

  players = [
    {
      "slot": 0,
      "playerId": 1063,
      "firstName": "Derek",
      "lastName": "Jeter",
      "shirt": 2,
      "primaryPosition": "SS",
      "secondaryPosition": "SS",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 1,
      "playerId": 1465,
      "firstName": "Jose",
      "lastName": "Molina",
      "shirt": 26,
      "primaryPosition": "C",
      "secondaryPosition": "1B",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 2,
      "playerId": 599,
      "firstName": "Shelley",
      "lastName": "Duncan",
      "shirt": 17,
      "primaryPosition": "1B",
      "secondaryPosition": "RF",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 3,
      "playerId": 1698,
      "firstName": "Andy",
      "lastName": "Pettitte",
      "shirt": 46,
      "primaryPosition": "P",
      "secondaryPosition": "RP",
      "bats": "L",
      "throws": "L"
    },
    {
      "slot": 4,
      "playerId": 1363,
      "firstName": "Hideki",
      "lastName": "Matsui",
      "shirt": 55,
      "primaryPosition": "LF",
      "secondaryPosition": "LF",
      "bats": "L",
      "throws": "R"
    },
    {
      "slot": 5,
      "playerId": 385,
      "firstName": "Joba",
      "lastName": "Chamberlain",
      "shirt": 62,
      "primaryPosition": "RP",
      "secondaryPosition": "P",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 6,
      "playerId": 1824,
      "firstName": "Mariano",
      "lastName": "Rivera",
      "shirt": 42,
      "primaryPosition": "RP",
      "secondaryPosition": "P",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 7,
      "playerId": 1849,
      "firstName": "Alex",
      "lastName": "Rodriguez",
      "shirt": 13,
      "primaryPosition": "3B",
      "secondaryPosition": "SS",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 8,
      "playerId": 2176,
      "firstName": "Billy",
      "lastName": "Traber",
      "shirt": 46,
      "primaryPosition": "RP",
      "secondaryPosition": "P",
      "bats": "L",
      "throws": "L"
    },
    {
      "slot": 9,
      "playerId": 1728,
      "firstName": "Jorge",
      "lastName": "Posada",
      "shirt": 20,
      "primaryPosition": "C",
      "secondaryPosition": "1B",
      "bats": "S",
      "throws": "R"
    },
    {
      "slot": 10,
      "playerId": 910,
      "firstName": "LaTroy",
      "lastName": "Hawkins",
      "shirt": 22,
      "primaryPosition": "RP",
      "secondaryPosition": "P",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 11,
      "playerId": 504,
      "firstName": "Johnny",
      "lastName": "Damon",
      "shirt": 18,
      "primaryPosition": "LF",
      "secondaryPosition": "CF",
      "bats": "L",
      "throws": "L"
    },
    {
      "slot": 12,
      "playerId": 1129,
      "firstName": "Ian",
      "lastName": "Kennedy",
      "shirt": 36,
      "primaryPosition": "P",
      "secondaryPosition": "RP",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 13,
      "playerId": 315,
      "firstName": "Melky",
      "lastName": "Cabrera",
      "shirt": 28,
      "primaryPosition": "CF",
      "secondaryPosition": "OF",
      "bats": "S",
      "throws": "L"
    },
    {
      "slot": 14,
      "playerId": 274,
      "firstName": "Brian",
      "lastName": "Bruney",
      "shirt": 33,
      "primaryPosition": "RP",
      "secondaryPosition": "P",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 15,
      "playerId": 336,
      "firstName": "Robinson",
      "lastName": "Cano",
      "shirt": 24,
      "primaryPosition": "2B",
      "secondaryPosition": "2B",
      "bats": "L",
      "throws": "R"
    },
    {
      "slot": 16,
      "playerId": 2252,
      "firstName": "Chien-Ming",
      "lastName": "Wang",
      "shirt": 40,
      "primaryPosition": "P",
      "secondaryPosition": "RP",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 17,
      "playerId": 741,
      "firstName": "Brett",
      "lastName": "Gardner",
      "shirt": 6,
      "primaryPosition": "CF",
      "secondaryPosition": "OF",
      "bats": "L",
      "throws": "L"
    },
    {
      "slot": 18,
      "playerId": 790,
      "firstName": "Alberto",
      "lastName": "Gonzalez",
      "shirt": 63,
      "primaryPosition": "SS",
      "secondaryPosition": "IF",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 19,
      "playerId": 1754,
      "firstName": "Ryan",
      "lastName": "Raburn",
      "shirt": 25,
      "primaryPosition": "LF",
      "secondaryPosition": "2B",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 20,
      "playerId": 2140,
      "firstName": "Clete",
      "lastName": "Thomas",
      "shirt": 18,
      "primaryPosition": "CF",
      "secondaryPosition": "OF",
      "bats": "L",
      "throws": "R"
    },
    {
      "slot": 21,
      "playerId": 1690,
      "firstName": "Jason",
      "lastName": "Perry",
      "shirt": 16,
      "primaryPosition": "RF",
      "secondaryPosition": "UT",
      "bats": "L",
      "throws": "R"
    },
    {
      "slot": 22,
      "playerId": 1013,
      "firstName": "Philip",
      "lastName": "Hughes",
      "shirt": 34,
      "primaryPosition": "P",
      "secondaryPosition": "RP",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 23,
      "playerId": 935,
      "firstName": "Felix",
      "lastName": "Hernandez",
      "shirt": 34,
      "primaryPosition": "P",
      "secondaryPosition": "RP",
      "bats": "R",
      "throws": "R"
    },
    {
      "slot": 24,
      "playerId": 1933,
      "firstName": "Humberto",
      "lastName": "Sanchez",
      "shirt": 77,
      "primaryPosition": "P",
      "secondaryPosition": "RP",
      "bats": "R",
      "throws": "R"
    }
  ];
  constructor() { }

  ngOnInit(): void {
  }
}
