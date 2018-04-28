import { Injectable } from '@angular/core';
import { TicketStub } from '../models/ticket-stub';
import { HttpClient, HttpParams, HttpHeaders, HttpRequest, HttpResponse } from '@angular/common/http';
import * as settings from '../app-settings';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class SearchService {

  constructor(
    private http: HttpClient
  ) {}

  search(
    term: string,
  ): Observable<TicketStub[]> {
    const params = {term: term};
    const ticketList = this.http.post<TicketStub[]>( settings.searchTerm, params);
    return ticketList.map(res => {
      return res;
    });
  }
}
