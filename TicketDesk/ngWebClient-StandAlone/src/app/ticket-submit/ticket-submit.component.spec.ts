import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketSubmitComponent } from './ticket-submit.component';

describe('TicketSubmitComponent', () => {
  let component: TicketSubmitComponent;
  let fixture: ComponentFixture<TicketSubmitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketSubmitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketSubmitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
