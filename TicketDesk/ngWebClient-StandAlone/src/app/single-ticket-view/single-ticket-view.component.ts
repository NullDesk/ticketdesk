import { Component, OnInit } from '@angular/core';
import { ActivityLogComponent } from '../activity-log/activity-log.component';
import { ContactInfoComponent } from '../contact-info/contact-info.component';
import { SingleTicketService } from '../services/single-ticket.service';
import { Ticket } from '../models/data';
import {Router, ActivatedRoute, Params} from '@angular/router';


@Component({
  selector: 'app-single-ticket-view',
  templateUrl: './single-ticket-view.component.html',
  styleUrls: ['./single-ticket-view.component.css']
})
export class SingleTicketViewComponent implements OnInit {

  ticket:Ticket = null;
  ticketId: number = null;
  userName: string = ''
  buttonTitles: Array<String> = null;
  public isCollapsed = true;

  constructor(private singleTicketService: SingleTicketService, 
    private activatedRoute: ActivatedRoute) { 
    this.activatedRoute.params.subscribe(params => {
      this.ticketId = params['ticketID'];
      this.userName = params['first_name'] + params['last_name']
    });
  }

  ngOnInit() {
    console.log('Starting Invidividual ticket view');
    this.ticket = 
      this.singleTicketService.getTicketDetails(this.ticketId);
    this.buttonTitles = 
      this.singleTicketService.getAvailableTicketActions(this.ticketId);
  }
}
