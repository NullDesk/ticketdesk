import { Injectable } from '@angular/core';
import { Ticket } from '../models/data';
import { tickets } from './ticket_db'

@Injectable()
export class MultiTicketService {

  constructor() { };

  filterList(
    listName: string,
    page?: number  
  ):{"ticketList":Ticket[], "maxPages": number}{

      let defaultOwner = "1000";
      let currentUser = defaultOwner;

      let myList:Ticket[] = [];

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

      let result = this.paginate(myList, page)


    // Open
    // Assigned 
    // Closed
    // Submitted
    // All
    return result
  }

  paginate(thisList:Ticket[], page?: number):{"ticketList":Ticket[],"maxPages": number }{

    let ticketsPerPage = 4;
    if(!page) page = 1;
    
    let myResult:{"ticketList":Ticket[],"maxPages": number } = {"ticketList": [], "maxPages": Math.ceil(thisList.length/ticketsPerPage)};
    
    
    let start = (page - 1) * ticketsPerPage;
    let finish = (page) * ticketsPerPage;
    for(var i = start ; i < finish; i++  ){

      myResult.ticketList.push(thisList[i]);
      console.log(myResult.ticketList[i]);
      if(thisList[i + 1] == null) break;
    }

    return myResult;
  }

  


}
