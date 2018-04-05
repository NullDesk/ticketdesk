import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { TicketActionEnum } from '../models/ticket-actions.constants';
import { Ticket } from '../models/ticket';
import { Entry } from '../models/entry';
import { catchError, retry } from 'rxjs/operators';
import * as settings from '../app-settings';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';

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

  getTicketDetails(ticketId: number): Observable<any> {
    return this.http.get(settings.ticketDetailsURL + ticketId.toString())
      .pipe(catchError(this.handleError));
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

  private handleError(error: HttpErrorResponse): ErrorObservable {
    if (error.error instanceof ErrorEvent) {
      // ... this is a client side error, handle it!
      console.error(`client error occurred: ${error.error.message}`)
    } else {
      // ... this is a server error!
      console.error(`server error occurred, status code ${error.status}`)
    }
    return new ErrorObservable('Experiencing some issues, we are sorry');
  }
}
