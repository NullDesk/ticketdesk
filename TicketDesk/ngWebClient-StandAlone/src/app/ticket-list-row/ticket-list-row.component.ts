import { Component, OnInit, Input } from '@angular/core';
import { ListTicket } from '../models/list-ticket';

@Component({
  selector: 'app-ticket-list-row',
  templateUrl: './ticket-list-row.component.html',
  styleUrls: ['./ticket-list-row.component.css']
})
export class TicketListRowComponent implements OnInit {
  @Input() ticket: ListTicket;
  constructor() { }

  ngOnInit() {
  }

}
