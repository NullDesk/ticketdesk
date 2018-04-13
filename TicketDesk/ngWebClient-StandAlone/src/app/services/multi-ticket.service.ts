import { Injectable } from '@angular/core';
import { Ticket } from '../models/ticket';
import { TicketStub } from '../models/ticket-stub';
import { HttpClient, HttpParams, HttpHeaders, HttpRequest, HttpResponse } from '@angular/common/http';
import * as settings from '../app-settings';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class MultiTicketService {

  constructor(
    private http: HttpClient
  ) {}


  // Get ticketList from index enpoint
  indexList(
    listName: string,
    page?: number
  ): Observable<TicketStub[]> {
    // const params = new HttpParams().set('listName', listName).set('page', String(page));
    const params = {page: page, listName: listName};
    const ticketList = this.http.post<TicketStub[]>( settings.getTicketsIndex, params);
    return ticketList.map(res => {
      console.warn('TicketList Responese', JSON.stringify(res));
      return res;
    });
  }

  resetFilterAndSort() {
    this.http.post( settings.resetTicketsFilterAndSort, '');
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
