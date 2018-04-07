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
      'Content-Type':  'application/json'
    })
  };

  // Get ticketList from index enpoint
  indexList(
    listName: string,
    page?: number
  ): Observable<ListTicket[]> {
    // const params = new HttpParams().set('listName', listName).set('page', String(page));
    const params = {page: page, listName: listName};
    const ticketList = this.http.post<ListTicket[]>( settings.getTicketsIndex, params, this.httpOptions);
    console.warn('This is what I got from the fucking Get call');
    console.warn(ticketList);
    return ticketList.map(res => {
      console.warn('TicketList Responese', JSON.stringify(res));
      return res;
    });
  }

  resetFilterAndSort() {
    this.http.post( settings.resetTicketsFilterAndSort, '', this.httpOptions);
  }

  sortList(
    page: number,
    listName: string,
    columnName: string,
    isMultiSort: boolean
  ) {

  }

  filterList(
    listName: string,
    page?: number,
    ticketStatus?: boolean,
    owner?: string,
    assignedTo?: string
  ) {

  }

}
