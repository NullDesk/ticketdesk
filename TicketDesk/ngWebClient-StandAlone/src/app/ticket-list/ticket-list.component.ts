import { Component, OnInit } from '@angular/core';
import { TICKETS } from '../models/mocks/mock-ticket-preview';
import { TicketPreview } from '../models/ticket-preview';
@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})
export class TicketListComponent  {

  ticketList: TicketPreview[] = TICKETS


}
