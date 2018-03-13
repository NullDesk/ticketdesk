import { Component, OnInit } from '@angular/core';
import { ActivityLogComponent } from '../activity-log/activity-log.component';
import { ContactInfoComponent } from '../contact-info/contact-info.component';
import { TicketActionEntryComponent } from '../ticket-action-entry/ticket-action-entry.component';
import { SingleTicketService } from '../services/single-ticket.service';
import { Ticket } from '../models/ticket';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-single-ticket-view',
  templateUrl: './single-ticket-view.component.html',
  styleUrls: ['./single-ticket-view.component.css']
})
export class SingleTicketViewComponent implements OnInit {
  ticket: Ticket = null;
  ticketId: number = null;
  ticketActionPermissions = 0;
  public isCollapsed = true;
  ticketStatus: string;
  constructor(private singleTicketService: SingleTicketService,
    private activatedRoute: ActivatedRoute) {
    this.activatedRoute.params.subscribe(params => {
      this.ticketId = Number(params['ticketID']);
    });
  }

  ticketIsOpen(): boolean {
    const status = this.ticketStatus.toLowerCase();
    return status.indexOf('active') !== -1
      || status.indexOf('moreinfo') !== -1;
  }

  ngOnInit() {
    this.ticket =
      this.singleTicketService.getTicketDetails(this.ticketId);
    this.ticketStatus = this.ticket.status;
  }
}
