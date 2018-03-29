import { Injectable, Host } from '@angular/core';
import { Ticket } from '../models/ticket';
import { Logs } from '../models/logs';
import { Entry } from '../models/entry';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { tickets, logs } from './ticket_db';
import * as settings from '../app-settings';

interface TicketPermissions {
  ticketPermissions: number;
}

@Injectable()
export class SingleTicketService {
  // error 500, success 200
  const 
      HOST: string = "http://localhost:50067/";
      END_POINT: string = "api/ticket/";
      PATH: String;
      api: HttpClient;
      ticket: Object;
  constructor(private http: HttpClient) {
    this.http.get(
      this.HOST.concat(this.END_POINT))
    .subscribe(data => {
      this.ticket = data; // need to validate against ticket model
      console.warn('ticket data:', this.ticket)
    })
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
  // todo: refactor this into the ticket itself
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

  getAvailableTicketActions(ticketId: number) {
    console.log('Calling getAvailableTicketActions');
    return this.http.get(
      settings.getValidActionsURL + ticketId.toString()
    );
  }
}
