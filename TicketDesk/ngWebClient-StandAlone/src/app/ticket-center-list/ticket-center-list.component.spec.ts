import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketCenterListComponent } from './ticket-center-list.component';

describe('TicketCenterListComponent', () => {
  let component: TicketCenterListComponent;
  let fixture: ComponentFixture<TicketCenterListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketCenterListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketCenterListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
