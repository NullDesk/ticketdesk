import {Ticket} from '../models/data';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';

@Injectable()
export class SubmitTicketService {

  constructor(private http: Http) {
  }

	submitTicket(tkt: Ticket) {
	console.log('ticket submitted:');
	console.log(tkt);
        // this is where the http stuff goes
		// then we get to pretend we got the id back from the response

		console.log('POST');
		const url = `http://localhost:50067/api/tickets`;
		this.http.post(url, tkt).subscribe(res => console.log(res.json()));

		this.http.get(url + '/1').subscribe(res => console.log(res.text()));
		return 1111;
	}
}
