import { Injectable, Component, OnInit } from '@angular/core';
import { TicketStub, ticketlistToUserDisplayMap } from '../models/ticket-stub';
import { userPermissions } from '../models/user-details';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MultiTicketService } from '../services/multi-ticket.service';
import { NgbTabChangeEvent } from '@ng-bootstrap/ng-bootstrap/tabset/tabset';

@Component({
  selector: 'app-ticket-center',
  templateUrl: './ticket-center.component.html',
  styleUrls: ['./ticket-center.component.css']
})


export class TicketCenterComponent implements OnInit {
  tabNames: string[] = ['mytickets', 'opentickets', 'historytickets'];
  ticketListResults: { ticketList: TicketStub[], maxPages: number } = { ticketList: undefined, maxPages: 2};
  currentList = '';
  listReady: Boolean = false;

  constructor(private multiTicketService: MultiTicketService) {
  }

  ngOnInit() {
    this.multiTicketService.getUserPermissions()
        .subscribe(permissions => {
          if (permissions === userPermissions.admin || permissions === userPermissions.resolver) {
            this.tabNames.unshift('unassigned', 'assignedToMe');
          }
        });
    // Sending empty string, gets the default page from the backend, dependent on their permissions.
    // mytickets for standard users, unassigned for resolvers and admins
    this.getTicketList(this.currentList, 1);
  }

  getTicketList(listName: string, page: number): void {
    this.listReady = false;
    console.log('Getting ticketlist for', listName, 'at page ', page);
    this.multiTicketService.indexList(listName, page)
        .subscribe(ticketList => {
          this.ticketListResults.ticketList = ticketList;
          this.ticketListResults.maxPages = page + 1;
          this.listReady = true; });
  }

  onTabChange(event: NgbTabChangeEvent) {
    this.currentList = event.nextId;
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
