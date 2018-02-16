import { Injectable } from '@angular/core';
import { Ticket, Logs, Entry } from '../models/data';
import { tickets, logs } from './ticket_db';

/**
 * DESIGN THOUGHTS:
 * Since this class will only deal with single tickets, I think we should inject
 * a class that actually makes calls to API (api.service.ts?), and assign a class
 * member var to that ticket so all functions have access to it and returning
 * various ticket properties will be handled simply
 */


@Injectable()
export class SingleTicketService {

  // probably need an injection from class that makes calls to API
  constructor() { 
    // should we get ticket also as injection? 
  }

  getTicketDetails(ticketId: number): Ticket {
    let get_ticket: Ticket = null; //

    for (let ticket of tickets) { // "search" database here to match ticketId
      if (ticket.ticketId == ticketId) {
        get_ticket = ticket;
        break;
      }
    }
    return get_ticket;
  }

  getOwner(ticketId: number) {
    // const ticket = 
  }

  getTicketFiles(ticketId: number) {

  }

  getTicketLog(ticketId: number): [Entry] {
    for (let log of logs) {
      console.log(`log.ticketId: ${log.ticketId}`);
      console.log(`ticketId: ${ticketId}`);

      if (log.ticketId == ticketId) {
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
    // available actions: AddComment (all users), Assign (staff/admin), Close, EditTicketInfo (regular), ForceClose (regular), GiveUp, 
    // ModifyAttachments (all users), Pass, RequestMoreInfo, ReOpen, Resolve, SupplyMoreInfo, TakeOver (staff/admin)
    const ticketActions = {
      1111: ['AddComment', 'ModifyAttachments', 'EditTicketInfo', 'ForceClose'],
      2222: ['AddComment', 'ModifyAttachments', 'Assign', 'TakeOver'],
      3333: ['AddComment', 'ModifyAttachments', 'Assign', 'TakeOver']
    }
    const actions: Array<String> = ticketActions[ticketId]
    return actions;
  }
}
