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


  // Currently this endpoint does not return maxPage.
  // So pagination is done one page at a time. Without knowledge of how many items there are.
  getTicketList(
    listName: string,
    page?: number
  ): Observable<TicketStub[]> {
    const params = {page: page, listName: listName};
    const ticketList = this.http.post<TicketStub[]>(settings.getTicketList, params);
    return ticketList;
  }

  resetFilterAndSort() {
    this.http.post(settings.resetTicketsFilterAndSort, '');
  }

  sortList(
    page: number,
    listName: string,
    columnName: string,
    isMultiSort: boolean
  ) {
    const params = {page: page, listName: listName, columnName: columnName, isMultiSort: isMultiSort};
    return this.http.post<TicketStub[]>(settings.getSortedTicketList, params);
  }

  filterList(
    listName: string,
    page?: number,
    ticketStatus?: boolean,
    ownerId?: string,
    assignedTo?: string
  ) {

  }
}
