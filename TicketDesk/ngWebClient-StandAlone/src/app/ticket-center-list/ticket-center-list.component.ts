import { Component, OnInit, Input } from '@angular/core';
import { MultiTicketService } from '../services/multi-ticket.service';
import { ListTicket } from '../models/list-ticket';

@Component({
  selector: 'app-ticket-center-list',
  template: `<app-ticket-list [ticketListResults]="ticketListResults"> </app-ticket-list>`

})
export class TicketCenterListComponent implements OnInit {
  @Input() listName: string;
  ticketListResults: { 'ticketList': ListTicket[], 'maxPages': number };

  constructor(private multiTicketService: MultiTicketService) {

  }

  ngOnInit() {
    this.ticketListResults = this.multiTicketService.filterList(this.listName);
  }


}
