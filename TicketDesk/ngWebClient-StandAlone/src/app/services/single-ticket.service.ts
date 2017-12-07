import { Injectable } from '@angular/core';
import { Ticket , Logs, Entry } from '../models/data';
import { tickets, logs } from './ticket_db'
@Injectable()
export class SingleTicketService {

  constructor() {};

  getTicketDetails(ticketId: number):Ticket{
    let get_ticket:Ticket = null; // 

    for(let ticket of tickets){ // "search" database here to match ticketId
      if(ticket.ticketId == ticketId){
        get_ticket = ticket;
        break;
      }
    };
    
	 return get_ticket; 
  };

  getTicketFiles(ticketId: number){

  };

  getTicketLog(ticketId: number):[Entry]{
    for(let log of logs){
      console.log("log.ticketId: " + log.ticketId);
      console.log("ticketId: " + ticketId)

      if (log.ticketId == ticketId) {
        console.log("Entered if statement");
        console.log("entries list: " + log.entries);
        return log.entries;
      }
    }
    return null;

  };

  changeTicketSubscription(ticketID: number){

  };

}
