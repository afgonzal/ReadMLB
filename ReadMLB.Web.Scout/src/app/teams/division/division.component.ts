import { Component, OnInit, Input } from '@angular/core';
import { DivisionModel } from '../team.model';

@Component({
  selector: 'app-division',
  templateUrl: './division.component.html',
  styleUrls: ['./division.component.scss']
})
export class DivisionComponent implements OnInit {
  @Input() division: DivisionModel;
  constructor() { }

  ngOnInit(): void {
  }

}
