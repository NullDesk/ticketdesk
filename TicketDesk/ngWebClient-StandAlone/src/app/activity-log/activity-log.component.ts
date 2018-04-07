import { Component, OnInit } from '@angular/core';
import { SingleTicketService } from '../services/single-ticket.service';
import { Ticket } from '../models/ticket';
import { Entry } from '../models/entry';
import {Router, ActivatedRoute, Params} from '@angular/router';

@Component({
  selector: 'app-activity-log',
  templateUrl: './activity-log.component.html',
  styleUrls: ['./activity-log.component.css']
})
export class ActivityLogComponent implements OnInit {
  ticketLog: Entry[] = [];
  ticketId: number = null;

  constructor(private singleTicketService: SingleTicketService, private activatedRoute: ActivatedRoute) {
    this.activatedRoute.params.subscribe(params => {
      this.ticketId = params['ticketID'];
    });
  }

  ngOnInit() {
    this.singleTicketService.getTicketLog(this.ticketId).subscribe(
      res => {
        console.warn("res.list is ", res.list);
        this.ticketLog = res.list;
      }
    );
  }

}
