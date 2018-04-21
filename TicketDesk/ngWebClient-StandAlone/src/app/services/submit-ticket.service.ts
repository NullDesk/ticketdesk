import { Ticket } from '../models/ticket';
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpRequest, HttpHeaders } from '@angular/common/http';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import * as settings from '../app-settings';

interface SubmissionResult {
  'httpCode': number; // this is, obviously, not an http code
  'ticketID': number;
  'errorMessage': string;
}

@Injectable()
export class SubmitTicketService {

  constructor(private http: HttpClient) {
  }

  submitTicket(tkt: Ticket) {
    return this.http.post<any>(settings.ticketDetailsURL, tkt)
      .map(res => {
        console.warn('this is the response to ticket submission', res);
        if (res['httpCode'] == 200) {
          return res['ticketID'];
        } else {
          return new ErrorObservable('An error occurred while submitting this ticket.');
          }
        });
      }
}
