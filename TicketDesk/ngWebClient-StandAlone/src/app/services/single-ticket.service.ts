import { Injectable } from '@angular/core';
import { Ticket, Logs, Entry } from '../models/data';
import { tickets, logs } from './ticket_db';

@Injectable()
export class SingleTicketService {

  constructor() { }

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

  getAvailableTicketActions(ticketId: number): number {
    // available actions: AddComment (all users), Assign (staff/admin), Close, EditTicketInfo (regular), ForceClose (regular), GiveUp, 
    // ModifyAttachments (all users), Pass, RequestMoreInfo, ReOpen, Resolve, SupplyMoreInfo, TakeOver (staff/admin)
    // const ticketActions = {
      // 1111: ['AddComment', 'ModifyAttachments', 'EditTicketInfo', 'ForceClose'],
      // 2222: ['AddComment', 'ModifyAttachments', 'Assign', 'TakeOver'],
      // 3333: ['AddComment', 'ModifyAttachments', 'Assign', 'TakeOver']
    // }
    // const actions: Array<String> = ticketActions[ticketId]
    return (2**17)-1;
  }
}
