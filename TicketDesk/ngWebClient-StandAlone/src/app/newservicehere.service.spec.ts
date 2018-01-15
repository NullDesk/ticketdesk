import { TestBed, inject } from '@angular/core/testing';

import { NewservicehereService } from './newservicehere.service';

describe('NewservicehereService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NewservicehereService]
    });
  });

  it('should be created', inject([NewservicehereService], (service: NewservicehereService) => {
    expect(service).toBeTruthy();
  }));
});
