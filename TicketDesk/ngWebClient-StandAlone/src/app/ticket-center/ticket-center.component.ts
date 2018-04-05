import { Injectable, Component, OnInit } from '@angular/core';
import { ListTicket } from '../models/list-ticket';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MultiTicketService } from '../services/multi-ticket.service';
import { NgbTabChangeEvent } from '@ng-bootstrap/ng-bootstrap/tabset/tabset';

@Component({
  selector: 'app-ticket-center',
  templateUrl: './ticket-center.component.html',
  styleUrls: ['./ticket-center.component.css']
})


export class TicketCenterComponent implements OnInit {
  ticketList: ListTicket[];
  tabNames: string[] = ['unassigned', 'assignedToMe', 'mytickets', 'opentickets', 'historytickets']; // Make input settings at some point
  ticketListResults: { 'ticketList': ListTicket[], 'maxPages': number };

  constructor(private multiTicketService: MultiTicketService) {
  }

  ngOnInit() {
    this.getTicketList('');
  }

  getTicketList(listName: string): void {
    this.multiTicketService.indexList(listName, 1)
        .subscribe(ticketList => this.ticketList = ticketList);
  }

  onTabChange(event: NgbTabChangeEvent) {
    console.log('getting ticket for => ', event.activeId);
    this.getTicketList(event.activeId);
  }

}
