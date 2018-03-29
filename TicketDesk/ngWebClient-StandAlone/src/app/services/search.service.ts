import { Injectable } from '@angular/core';
import { Ticket } from '../models/ticket';
import { tickets } from './ticket_db';

@Injectable()
export class SearchService {

  constructor() { }

  search(
    term: string
  ): {'ticketList': Ticket[], 'maxPages': number} {
    // Returning dummy data of all tickets
    return {'ticketList': tickets, 'maxPages': 1};
  }
}
