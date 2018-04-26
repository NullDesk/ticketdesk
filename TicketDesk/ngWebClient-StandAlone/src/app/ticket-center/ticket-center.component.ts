import { Injectable, Component, OnInit } from '@angular/core';
import { TicketStub, ticketlistToUserDisplayMap } from '../models/ticket-stub';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MultiTicketService } from '../services/multi-ticket.service';
import { NgbTabChangeEvent } from '@ng-bootstrap/ng-bootstrap/tabset/tabset';

@Component({
  selector: 'app-ticket-center',
  templateUrl: './ticket-center.component.html',
  styleUrls: ['./ticket-center.component.css']
})


export class TicketCenterComponent implements OnInit {
  tabNames: string[] = ['unassigned', 'assignedToMe', 'mytickets', 'opentickets', 'historytickets']; // Make input settings at some point
  ticketListResults: { ticketList: TicketStub[], maxPages: number } = { ticketList: undefined, maxPages: 2};
  currentList = '';
  listReady: Boolean = false;

  constructor(private multiTicketService: MultiTicketService) {
  }

  ngOnInit() {
    // Sending empty string, gets the default page from the backend, dependent on their permissions.
    this.getTicketList(this.currentList, 1);
  }

  getTicketList(listName: string, page: number): void {
    this.listReady = false;
    this.currentList = listName;
    console.log('Getting ticketlist for', listName, 'at page ', page);
    this.multiTicketService.indexList(listName, page)
        .subscribe(ticketList => {
          this.ticketListResults.ticketList = ticketList;
          this.ticketListResults.maxPages++;
          this.listReady = true; });
  }

  onTabChange(event: NgbTabChangeEvent) {
    this.ticketListResults.maxPages = 2;
    console.log('getting ticket for => ', event.nextId);
    this.getTicketList(event.nextId, 1);
  }

  pageChange(page: number) {
    this.getTicketList(this.currentList, page);
  }

  convertListNameToDisplayStr(str: string) {
    return ticketlistToUserDisplayMap.has(str) ? ticketlistToUserDisplayMap.get(str) : str;
  }
}
