import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerDefenseComponent } from './player-defense.component';

describe('PlayerDefenseComponent', () => {
  let component: PlayerDefenseComponent;
  let fixture: ComponentFixture<PlayerDefenseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayerDefenseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayerDefenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
