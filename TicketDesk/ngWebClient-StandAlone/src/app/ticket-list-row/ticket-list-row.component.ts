import { Component,  Input } from '@angular/core';
import { Ticket } from '../models/data';


@Component({
  selector: 'app-ticket-list-row',
  templateUrl: './ticket-list-row.component.html',
  styleUrls: ['./ticket-list-row.component.css']
})
export class TicketListRowComponent {
  @Input() row: Ticket; // Currently doesn't work
  //ticket: TicketPreview = TICKET;
  //@Output() onRowClick = new EventEmitter<TicketPreview>(); //TODO implement this in HTML Will be more complex for selecting and going to ticket

}
