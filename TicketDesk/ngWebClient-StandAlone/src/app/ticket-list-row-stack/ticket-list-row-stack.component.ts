import { Component, OnInit, Input } from '@angular/core';
import { Ticket } from '../models/data';

@Component({
  selector: 'app-ticket-list-row-stack',
  templateUrl: './ticket-list-row-stack.component.html',
  styleUrls: ['./ticket-list-row-stack.component.css']
})
export class TicketListRowStackComponent implements OnInit {
  @Input() ticketList: Ticket[]; 
  constructor() { }

  ngOnInit() {
  }

}
