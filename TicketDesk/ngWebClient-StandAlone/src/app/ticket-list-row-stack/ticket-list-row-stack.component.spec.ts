import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketListRowStackComponent } from './ticket-list-row-stack.component';

describe('TicketListRowStackComponent', () => {
  let component: TicketListRowStackComponent;
  let fixture: ComponentFixture<TicketListRowStackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketListRowStackComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketListRowStackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
