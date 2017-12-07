import { Component, OnInit } from '@angular/core';
import { ActivityLogComponent } from '../activity-log/activity-log.component';
import { ContactInfoComponent } from '../contact-info/contact-info.component'
import { SingleTicketService } from '../services/single-ticket.service';
import { Ticket } from '../models/data';
@Component({
  selector: 'app-individual-ticket-view',
  templateUrl: './individual-ticket-view.component.html',
  styleUrls: ['./individual-ticket-view.component.css']
})
export class IndividualTicketViewComponent implements OnInit {

  single_ticket:Ticket = null;
  constructor(private singleTicketService : SingleTicketService) { }

  ngOnInit() {
    console.log('Starting Invidividual ticket view');
    this.single_ticket = this.singleTicketService.getTicketDetails(123456);
    console.log('Single Ticket: = ' + this.single_ticket.details);
    
  }
}
