import { Component, OnInit } from '@angular/core';
import { SingleTicketService } from '../services/single-ticket.service';
import { Ticket, Logs, Entry } from '../models/data';

@Component({
  selector: 'app-activity-log',
  templateUrl: './activity-log.component.html',
  styleUrls: ['./activity-log.component.css']
})
export class ActivityLogComponent implements OnInit {

  ticket_log:[Entry] = null;


  constructor(private singleTicketService : SingleTicketService) { }

  ngOnInit() {

  	this.ticket_log = this.singleTicketService.getTicketLog(123456);
  	console.log('Ticket log: ' + this.ticket_log)

  }

}
