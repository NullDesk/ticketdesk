import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IndividualTicketViewComponent } from './individual-ticket-view.component';

describe('IndividualTicketViewComponent', () => {
  let component: IndividualTicketViewComponent;
  let fixture: ComponentFixture<IndividualTicketViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IndividualTicketViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IndividualTicketViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
