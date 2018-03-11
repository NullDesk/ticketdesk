import { Injectable } from '@angular/core';
import { Ticket } from '../models/ticket';
import { Logs } from '../models/logs';
import { Entry } from '../models/entry';
import { tickets, logs } from './ticket_db';

@Injectable()
export class SingleTicketService {

  constructor() {
  }

  getTicketDetails(ticketId: number): Ticket {
    let getTicket: Ticket = null;
    for (const ticket of tickets) {
      if (ticket.ticketId === ticketId) {
        getTicket = ticket;
        break;
      }
    }
    return getTicket;
  }

  getOwner(ticketId: number) {

  }

  getTicketFiles(ticketId: number) {

  }

  getTicketLog(ticketId: number): Entry[] {
    for (const log of logs) {
      if (log.ticketId === ticketId) {
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
    return (2 ** 17) - 1;
  }
}
