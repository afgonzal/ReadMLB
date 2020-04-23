import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PitcherComponent } from './pitcher.component';

describe('PitcherComponent', () => {
  let component: PitcherComponent;
  let fixture: ComponentFixture<PitcherComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PitcherComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PitcherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
