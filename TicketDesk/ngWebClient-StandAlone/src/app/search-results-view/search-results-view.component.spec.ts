import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchResultsViewComponent } from './search-results-view.component';

describe('SearchResultsViewComponent', () => {
  let component: SearchResultsViewComponent;
  let fixture: ComponentFixture<SearchResultsViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchResultsViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchResultsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
