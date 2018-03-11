import { TestBed, inject } from '@angular/core/testing';

import { MultiTicketService } from './multi-ticket.service';

describe('MultiTicketService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MultiTicketService]
    });
  });

  it('should be created', inject([MultiTicketService], (service: MultiTicketService) => {
    expect(service).toBeTruthy();
  }));
});
