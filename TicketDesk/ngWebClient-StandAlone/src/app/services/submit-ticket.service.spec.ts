import { TestBed, inject } from '@angular/core/testing';

import { SubmitTicketService } from './submit-ticket.service';

describe('SubmitTicketService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SubmitTicketService]
    });
  });

  it('should be created', inject([SubmitTicketService], (service: SubmitTicketService) => {
    expect(service).toBeTruthy();
  }));
});
