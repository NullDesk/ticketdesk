import {Ticket} from '../models/data';
import { Injectable } from '@angular/core';

@Injectable()
export class SubmitTicketService {

  constructor() {
  }

	submitTicket(tkt: Ticket) {
	console.log("ticket submitted:");
	console.log(tkt);
        // this is where the http stuff goes
		// then we get to pretend we got the id back from the response
		return 1111;
	}
}
