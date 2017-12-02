import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketDetailEditorComponent } from './ticket-detail-editor.component';

describe('TicketDetailEditorComponent', () => {
  let component: TicketDetailEditorComponent;
  let fixture: ComponentFixture<TicketDetailEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketDetailEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketDetailEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
