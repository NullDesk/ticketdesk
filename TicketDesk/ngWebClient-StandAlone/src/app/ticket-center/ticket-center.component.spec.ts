import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketCenterComponent } from './ticket-center.component';

describe('TicketCenterComponent', () => {
  let component: TicketCenterComponent;
  let fixture: ComponentFixture<TicketCenterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketCenterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketCenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
