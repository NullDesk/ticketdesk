import { Component, OnInit } from '@angular/core';
import { Ticket, BLANK_TICKET } from '../models/data';
import { TicketDetailEditorComponent } from '../ticket-detail-editor/ticket-detail-editor.component';

@Component({
  selector: 'app-ticket-submit',
  templateUrl: './ticket-submit.component.html',
  styleUrls: ['./ticket-submit.component.css']
})
export class TicketSubmitComponent implements OnInit {
  private initialTicket: Ticket = BLANK_TICKET;
  constructor() { 
  }

  ngOnInit() {
  }

}
