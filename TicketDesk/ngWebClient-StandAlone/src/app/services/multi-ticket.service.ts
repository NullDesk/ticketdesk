import { Injectable } from '@angular/core';
import { Ticket } from '../models/ticket';
import { ListTicket } from '../models/list-ticket';
import { tickets } from './ticket_db';
import { HttpClient, HttpParams, HttpHeaders, HttpRequest, HttpResponse } from '@angular/common/http';
import * as settings from '../app-settings';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class MultiTicketService {

  constructor(
    private http: HttpClient
  ) {
  }

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
      'Authorization': 'my-auth-token'
    })
  };

  filterList(
    listName: string,
    page?: number
  ): Observable<ListTicket[]> {

      // const params = new HttpParams().set('listName', listName).set('page', String(page));
      const params = {page: page, listName: listName};
      const ticketList = this.http.post<ListTicket[]>( settings.getTicketsIndex, params);
      console.warn('This is what I got from the fucking Get call');
      console.warn(ticketList);
      ticketList.forEach( x => console.warn(x) );
      return ticketList;
     /*
      // OLD MOCK TRASH
      const defaultOwner = '1000';
      const currentUser = defaultOwner;

      const myList: Ticket[] = [];

      for (const ticket of tickets) {
        if (listName === 'Open' ) {
          if (ticket.status === 'open') {
            myList.push(ticket);
          }
        } else if (listName === 'Closed') {
          if (ticket.status === 'closed') {
            myList.push(ticket);
          }
        } else if (listName === 'Assigned') {
          if (currentUser === ticket.assignedTo) {
            myList.push(ticket);
          }
        } else if (listName === 'Submitted') {
          if (currentUser === ticket.ownerId) {
            myList.push(ticket);
          }

        } else {
          myList.push(ticket);
        }
      }

      const result = this.paginate(ticketList, page);


    // Open
    // Assigned
    // Closed
    // Submitted
    // All
    return result;
    */
  }

  paginate(thisList: ListTicket[], page?: number): {'ticketList': ListTicket[], 'maxPages': number } {

    const ticketsPerPage = 4;
    if (!page) { page = 1; }

    const myResult: {'ticketList': ListTicket[],
        'maxPages': number } = {'ticketList': [],
          'maxPages': Math.ceil(thisList.length / ticketsPerPage)};


    const start = (page - 1) * ticketsPerPage;
    const finish = (page) * ticketsPerPage;
    for (let i = start ; i < finish; i++  ) {

      myResult.ticketList.push(thisList[i]);
      console.log(myResult.ticketList[i]);
      if (thisList[i + 1] === null) { break; }
    }

    return myResult;
  }




}
