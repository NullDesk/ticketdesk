import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SubmitTicketService } from '../services/submit-ticket.service';
import { Ticket, BLANK_TICKET } from '../models/ticket';
import { TicketDetailEditorComponent } from '../ticket-detail-editor/ticket-detail-editor.component';

@Component({
  selector: 'app-ticket-submit',
  templateUrl: './ticket-submit.component.html',
  styleUrls: ['./ticket-submit.component.css'],
  providers: [SubmitTicketService]
})
export class TicketSubmitComponent implements OnInit {
  private initialTicket: Ticket = BLANK_TICKET;
  constructor(
    private router: Router,
    private sts: SubmitTicketService,
  ) {
  }

  receiveTicket(ticket) {
    // do the ticket
    // get back the ID
    console.warn('we got a ticket emitted', ticket);
    this.sts.submitTicket(ticket).subscribe( res => {
      console.warn('THIS IS WHAT WE GOT BACK', res);
      if (res['ticketID']) {
        this.router.navigate(['/ticket/' + res['ticketID'].toString()]);
      } else {
        this.router.navigate(['OH_NO_OH_NO_OH_NO']);
      }
    });
  }

  ngOnInit() {
  }

}
