import { Injectable } from '@angular/core';
import { Ticket, Logs, Entry } from '../models/data';
import { tickets, logs } from './ticket_db';

@Injectable()
export class SingleTicketService {

  constructor() {
  }

  getTicketDetails(ticketId: number): Ticket {
    let get_ticket: Ticket = null;

    for (const ticket of tickets) { // "search" database here to match ticketId
      if (ticket.ticketId === ticketId) {
        get_ticket = ticket;
        break;
      }
    }
    return get_ticket;
  }

  getOwner(ticketId: number) {
  // todo: refactor this into the ticket itself
  }

  getTicketFiles(ticketId: number) {

  }

  getTicketLog(ticketId: number): [Entry] {
    for (const log of logs) {
      console.log(`log.ticketId: ${log.ticketId}`);
      console.log(`ticketId: ${ticketId}`);

      if (log.ticketId === ticketId) {
        console.log('Entered if statement');
        console.log(`entries list: ${log.entries}`);
        return log.entries;
      }
    }
    return null;
  }

  changeTicketSubscription(ticketID: number) {

  }

  getAvailableTicketActions(ticketId: number): Array<String> {
    const ticketActions = {
      1111: ['AddComment', 'ModifyAttachments', 'EditTicketInfo', 'ForceClose'],
      2222: ['AddComment', 'ModifyAttachments', 'Assign', 'TakeOver'],
      3333: ['AddComment', 'ModifyAttachments', 'Assign', 'TakeOver']
    };
    const actions: Array<String> = ticketActions[ticketId];
    return actions;
  }
}
