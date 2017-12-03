import { Injectable } from '@angular/core';
import { Ticket } from '../models/data';
import { tickets } from './ticket_db'
@Injectable()
export class SingleTicketService {

  constructor() { };
  getTicketDetails(ticketId: number) {
    let get_ticket:Ticket = null;
    let ticket:any;
    for(ticket in tickets){
      if(ticket.ticketId == ticketId){
        get_ticket = ticket;
        break;
      }
    };
	 return get_ticket; 
  }

}
