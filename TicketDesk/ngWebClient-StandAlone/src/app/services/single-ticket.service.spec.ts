import { TestBed, inject } from '@angular/core/testing';

import { SingleTicketService } from './single-ticket.service';

describe('SingleTicketService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SingleTicketService]
    });
  });

  it('should be created', inject([SingleTicketService], (service: SingleTicketService) => {
    expect(service).toBeTruthy();
  }));
});
