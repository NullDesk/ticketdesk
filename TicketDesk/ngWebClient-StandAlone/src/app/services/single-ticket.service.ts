import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { TicketActionEnum } from '../models/ticket-actions.constants';
import { Ticket } from '../models/ticket';
import { Logs } from '../models/logs';
import { Entry } from '../models/entry';
import { tickets, logs } from './ticket_db';
import * as settings from '../app-settings';

interface TicketPermissions {
  ticketPermissions: number;
}

interface EventList {
  events: Entry[];
}

@Injectable()
export class SingleTicketService {

  constructor(
    private http: HttpClient
  ) {
  }

  getTicketDetails(ticketId: number): Observable<Object> {
    return this.http.get(settings.ticketDetailsURL + ticketId.toString());
  }

  getOwner(ticketId: number) {
  // todo: refactor this into the ticket itself
  }

  getTicketFiles(ticketId: number) {

  }

  getTicketLog(ticketId: number): Observable<Entry[]> {
    return this.http.get<EventList>(
      settings.ticketEventsURL + ticketId.toString()
    ).map(res => {
      if (res['events']) {
        return res['events'];
      } // todo: actual error handling here
    });
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
