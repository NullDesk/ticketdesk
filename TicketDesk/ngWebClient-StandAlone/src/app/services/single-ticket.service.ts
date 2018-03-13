import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Ticket, Logs, Entry } from '../models/data';
import { TicketActionEnum } from '../models/ticket-actions.constants';
import { tickets, logs } from './ticket_db';
import * as settings from '../app-settings';

interface TicketPermissions {
  ticketPermissions: number;
}

@Injectable()
export class SingleTicketService {

  constructor(
    private http: HttpClient
  ) {
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

  submitTicketAction(value: object, action: TicketActionEnum) {
    console.log('performing ' + action.displayText);
    return this.http.post<Object>(action.getURL(), value);
  }
  getAvailableTicketActions(ticketId: number) {
    console.log('Calling getAvailableTicketActions');

    return this.http.get(
      settings.getValidActionsURL + ticketId.toString()
    );
  }
}
