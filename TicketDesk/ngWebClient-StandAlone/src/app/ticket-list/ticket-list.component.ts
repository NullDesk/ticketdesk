import { Component, OnInit } from '@angular/core';
import { TICKET } from '../models/mocks/mock-ticket-preview'
import { TicketPreview } from 'app/models/ticket-preview';
@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})
export class TicketListComponent implements OnInit {

  aTicket:TicketPreview = TICKET
  ticketList = [TICKET]
  constructor() { }

  ngOnInit() {
  }

}
