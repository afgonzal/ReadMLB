import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-rosters',
  templateUrl: './rosters.component.html',
  styleUrls: ['./rosters.component.scss']
})
export class RostersComponent implements OnInit {

  constructor() { }
  players: [
    { playerId: 1, firstName: 'Ale', lastName: 'Gonzalez', slot: 0, playerType: 1},
    { playerId: 2, firstName: 'Ale', lastName: 'Gonzalez', slot: 1, playerType: 1},
    { playerId: 3, firstName: 'Ale', lastName: 'Gonzalez', slot: 2, playerType: 2},
    { playerId: 4, firstName: 'Ale', lastName: 'Gonzalez', slot: 3, playerType: 1},
    { playerId: 5, firstName: 'Ale', lastName: 'Gonzalez', slot: 4, playerType: 1},
    { playerId: 6, firstName: 'Ale', lastName: 'Gonzalez', slot: 5, playerType: 2},
    { playerId: 7, firstName: 'Ale', lastName: 'Gonzalez', slot: 6, playerType: 1},
    { playerId: 8, firstName: 'Ale', lastName: 'Gonzalez', slot: 7, playerType: 1},
  ];
  ngOnInit(): void {
  }

}
