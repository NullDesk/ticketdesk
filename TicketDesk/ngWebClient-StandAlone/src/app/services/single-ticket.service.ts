import { Injectable } from '@angular/core';
import { Ticket } from '../models/data';
import { tickets } from './ticket_db'
@Injectable()
export class SingleTicketService {

  constructor() {
   };
  getTicketDetails(ticketId: number):Ticket{
    let get_ticket:Ticket = null; // 

    for(let ticket of tickets){ // "search" database here to match ticketId
      if(ticket.ticketId == ticketId){
        get_ticket = ticket;
        break;
      }
    };
    
	 return get_ticket; 
  }

  getTicketFiles(ticketId: number){

  }

  getTicketLog(ticketId: number){
    /*let get_log:Logs = null;

    for(let log of logs){
      if(log.ticketId == ticketId){
        get_log = log.entries;
        break;
      }
    }

    return get_log;*/

  };

  changeTicketSubscription(ticketID: number){

  }

}
