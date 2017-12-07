import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdUserComponent } from './ad-user.component';

describe('AdUserComponent', () => {
  let component: AdUserComponent;
  let fixture: ComponentFixture<AdUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
