import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AttachFileComponent } from './attach-file.component';

describe('AttachFileComponent', () => {
  let component: AttachFileComponent;
  let fixture: ComponentFixture<AttachFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AttachFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AttachFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
