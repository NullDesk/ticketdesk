import { Ticket } from '../models/data';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import * as settings from '../app-settings';

@Injectable()
export class SubmitTicketService {

  constructor(private http: Http) {
  }

  submitTicket(tkt: Ticket) {
  console.log(tkt);
  console.log('POST');
  return this.http.post(settings.submitTicketURL, tkt)
  }
}
