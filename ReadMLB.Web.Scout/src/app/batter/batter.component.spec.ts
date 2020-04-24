import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BatterComponent } from './batter.component';

describe('BatterComponent', () => {
  let component: BatterComponent;
  let fixture: ComponentFixture<BatterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BatterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BatterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
