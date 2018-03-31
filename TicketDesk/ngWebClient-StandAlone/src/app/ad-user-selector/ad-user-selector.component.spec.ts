import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdUserSelectorComponent } from './ad-user-selector.component';

describe('AdUserSelectorComponent', () => {
  let component: AdUserSelectorComponent;
  let fixture: ComponentFixture<AdUserSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdUserSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdUserSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
