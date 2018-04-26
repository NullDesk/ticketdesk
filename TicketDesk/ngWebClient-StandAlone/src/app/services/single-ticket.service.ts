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

  constructor(private http: HttpClient) {}

  getTicketDetails(ticketId: number): Observable<any> {
    const url = settings.ticketDetailsURL + ticketId.toString()
    console.warn(`getting details for ticketId: ${ticketId} from url: ${url}`)
    return this.http.get(url)
      .pipe(catchError(this.handleError));
  }

  getTicketLog(ticketId: number): Observable<any> {
    return this.http.get(
      settings.ticketEventsURL + ticketId.toString()
    ).map(res => res['list']);
  }

  changeTicketSubscription(ticketID: number) {

  }

  submitTicketAction(value: object, action: TicketActionEnum) {
    console.log('performing ' + action.displayText);
    return this.http.post<Object>(action.getURL(), value);
  }

  getAvailableTicketActions(ticketId: number): Observable<number> {
    console.log('Calling getAvailableTicketActions');
    return this.http.get<number>(
      settings.getValidActionsURL + ticketId.toString()
    );
  }
  private handleError(error: HttpErrorResponse): ErrorObservable {
    if (error.error instanceof ErrorEvent) {
      // ... this is a client side error, handle it!
      console.error(`client error occurred: ${error.error.message}`);
    } else {
      // ... this is a server error!
      console.error(`server error occurred, status code ${error.status}`);
    }
    return new ErrorObservable('Experiencing some issues, we are sorry');
  }
}
