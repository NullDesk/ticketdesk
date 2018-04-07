import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketActionEntryComponent } from './ticket-action-entry.component';

describe('TicketActionEntryComponent', () => {
  let component: TicketActionEntryComponent;
  let fixture: ComponentFixture<TicketActionEntryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketActionEntryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketActionEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
