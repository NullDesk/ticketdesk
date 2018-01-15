import { TestBed, inject } from '@angular/core/testing';

import { AdContactService } from './ad-contact.service';

describe('AdContactService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdContactService]
    });
  });

  it('should be created', inject([AdContactService], (service: AdContactService) => {
    expect(service).toBeTruthy();
  }));
});
