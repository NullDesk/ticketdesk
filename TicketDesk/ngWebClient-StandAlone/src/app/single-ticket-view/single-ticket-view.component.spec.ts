import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleTicketViewComponent } from './single-ticket-view.component';

describe('SingleTicketViewComponent', () => {
  let component: SingleTicketViewComponent;
  let fixture: ComponentFixture<SingleTicketViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SingleTicketViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SingleTicketViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
