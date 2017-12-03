import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketListRowComponent } from './ticket-list-row.component';

describe('TicketListRowComponent', () => {
  let component: TicketListRowComponent;
  let fixture: ComponentFixture<TicketListRowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketListRowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketListRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
