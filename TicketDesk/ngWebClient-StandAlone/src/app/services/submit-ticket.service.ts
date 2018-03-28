import {Ticket} from '../models/data';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
@Injectable()
export class SubmitTicketService {

  constructor(private http: Http) {
  }

  submitTicket(tkt: Ticket) {
  console.log(tkt);
  console.log('POST');
  const url = `http://localhost:50067/api/tickets`;
  return this.http.post(url, tkt)
  }
}
