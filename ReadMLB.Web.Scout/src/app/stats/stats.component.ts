import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.scss']
})
export class StatsComponent implements OnInit {
  filterForm: FormGroup;
  leagues = [0, 1, 2];
  years = [2008, 2009];
  constructor() { }

  ngOnInit(): void {
    this.filterForm = new FormGroup({
      league: new FormControl(0),
      year: new FormControl(+localStorage.getItem('currentYear'))
    });
  }

  onFilter() {

  }
}
