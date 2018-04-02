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
  ticketList: ListTicket[];

  constructor(private multiTicketService: MultiTicketService) {

  }

  ngOnInit() {
    this.getTicketList();
    this.ticketListResults = {'ticketList': this.ticketList, 'maxPages': 1};
  }

  getTicketList(): void {
    this.multiTicketService.filterList(this.listName, 1)
        .subscribe(ticketList => this.ticketList = ticketList);
  }


}
