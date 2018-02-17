import { Component, OnInit } from '@angular/core';
import { ActivityLogComponent } from '../activity-log/activity-log.component';
import { ContactInfoComponent } from '../contact-info/contact-info.component';
import { TicketActionEntryComponent } from '../ticket-action-entry/ticket-action-entry.component';
import { SingleTicketService } from '../services/single-ticket.service';
import { Ticket } from '../models/data';
import {Router, ActivatedRoute, Params} from '@angular/router';


@Component({
  selector: 'app-individual-ticket-view',
  templateUrl: './individual-ticket-view.component.html',
  styleUrls: ['./individual-ticket-view.component.css']
})
export class IndividualTicketViewComponent implements OnInit {

  single_ticket:Ticket = null;
  ticketId: number = null;
	ticketActionPermissions: number = 0;
  public isCollapsed = true;

  constructor(private singleTicketService: SingleTicketService, private activatedRoute: ActivatedRoute ) { 
    this.activatedRoute.params.subscribe(params => {
      this.ticketId = params['ticketID'];
  });
  }

  ngOnInit() {
    this.single_ticket = this.singleTicketService.getTicketDetails(this.ticketId);
    this.ticketActionPermissions = this.singleTicketService.getAvailableTicketActions(this.ticketId);
  }
}
