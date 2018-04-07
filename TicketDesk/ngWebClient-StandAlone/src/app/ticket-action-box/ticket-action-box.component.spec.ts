import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketActionBoxComponent } from './ticket-action-box.component';

describe('TicketActionBoxComponent', () => {
  let component: TicketActionBoxComponent;
  let fixture: ComponentFixture<TicketActionBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketActionBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketActionBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
