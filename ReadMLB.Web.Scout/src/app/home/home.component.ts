import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  private currentOrganization = 'NYY';
  aaa = 'SWB';
  aa = 'TRE';
  constructor() { }

  ngOnInit() {
  }

}
