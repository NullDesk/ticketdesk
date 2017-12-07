import { Injectable } from '@angular/core';
import { Ticket } from '../models/data';
import { tickets } from './ticket_db'

@Injectable()
export class MultiTicketService {

  constructor() { };

  FilterList(
    listName: string,
    page?: number  
  )
    :[Ticket]{

      let defaultOwner = "1000";
      let currentUser = defaultOwner;

      let myList =  [];

      for(let ticket of tickets){
        if(listName == "Open" ){
          if(ticket.status == "open"){
            myList.push(ticket);
          }
        }
        else if (listName == "Closed"){
          if(ticket.status == "closed"){
            myList.push(ticket);
          }
        }
        else if (listName == "Assigned"){
          if(currentUser == ticket.assignedTo){
            myList.push(ticket);
          }
        }
        else if (listName == "Submitted"){
          if(currentUser == ticket.owner){
            myList.push(ticket);
          }

        }
        else {
          myList.push(ticket);
        }
      }


    // Open
    // Assigned 
    // Closed
    // Submitted
    // All

    return tickets
  }

  


}
