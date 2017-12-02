import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FoobarComponent } from './foobar.component';

describe('FoobarComponent', () => {
  let component: FoobarComponent;
  let fixture: ComponentFixture<FoobarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FoobarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FoobarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
