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
  ticketListResults: { ticketList: TicketStub[], maxPages: number } = { ticketList: undefined, maxPages: 1};
  listReady: Boolean = false;

  constructor(private multiTicketService: MultiTicketService) {
  }

  ngOnInit() {
    this.getTicketList('');
    this.ticketListResults.maxPages = 1;
  }

  getTicketList(listName: string): void {
    console.log('Getting ticketlist for', listName);
    this.multiTicketService.indexList(listName, 1)
        .subscribe(ticketList => {
          this.ticketListResults.ticketList = ticketList;
          this.listReady = true; });
  }

  public onTabChange(event: NgbTabChangeEvent) {
    this.listReady = false;
    console.log('getting ticket for => ', event.nextId);
    this.getTicketList(event.nextId);
  }

  public userifyString(str: string) {
    return ticketlistToUserDisplayMap.get(str);
  }

}
