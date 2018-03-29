import { Component, OnInit } from '@angular/core';
import { SingleTicketService } from '../services/single-ticket.service';
import { Ticket } from '../models/ticket';
import { Logs } from '../models/logs';
import { Entry } from '../models/entry';
import {Router, ActivatedRoute, Params} from '@angular/router';

@Component({
  selector: 'app-activity-log',
  templateUrl: './activity-log.component.html',
  styleUrls: ['./activity-log.component.css']
})
export class ActivityLogComponent implements OnInit {
  ticket_log: Entry[] = null;
  ticketId: number = null;

  constructor(private singleTicketService: SingleTicketService, private activatedRoute: ActivatedRoute) {
    this.activatedRoute.params.subscribe(params => {
      this.ticketId = params['ticketID'];
    });
  }

  ngOnInit() {

    this.ticket_log = this.singleTicketService.getTicketLog(this.ticketId);
    console.log('Ticket log: ' + this.ticket_log);

  }

}
