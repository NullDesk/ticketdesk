import {
  Component, OnInit, Input,
  OnChanges, SimpleChanges, SimpleChange
} from '@angular/core';
import { MultiTicketService } from '../services/multi-ticket.service';
import { Ticket } from '../models/ticket';

@Component({
  selector: 'app-ticket-center-list',
  template: `<app-ticket-list [ticketListResults]="ticketListResults"> </app-ticket-list>`

})
export class TicketCenterListComponent implements OnChanges, OnInit {
  @Input() listName: string;
  ticketListResults: { 'ticketList': Ticket[], 'maxPages': number };

  constructor(private multiTicketService: MultiTicketService) {

  }
  ngOnChanges(changes: SimpleChanges) {
    const name: SimpleChange = changes.name;
    this.ticketListResults = this.multiTicketService.filterList(this.listName);
  }

  ngOnInit() {
    this.ticketListResults = this.multiTicketService.filterList(this.listName);
  }


}
