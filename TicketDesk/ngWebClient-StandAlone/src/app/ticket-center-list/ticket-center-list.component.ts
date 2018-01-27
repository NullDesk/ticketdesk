import { Component, OnInit, Input,
         OnChanges, SimpleChanges, SimpleChange 
       } from '@angular/core';
import { MultiTicketService } from '../services/multi-ticket.service';
import { Ticket } from '../models/data';

@Component({
  selector: 'app-ticket-center-list',
  templateUrl: './ticket-center-list.component.html',
  styleUrls: ['./ticket-center-list.component.css']
})
export class TicketCenterListComponent implements OnChanges, OnInit {
  @Input() listName: string;
  ticketListResults: {"ticketList":Ticket[], "maxPages" : number};
  
  constructor(private multiTicketService : MultiTicketService) { 
    
  }
  ngOnChanges(changes: SimpleChanges){
    const name: SimpleChange = changes.name;
    this.ticketListResults = this.multiTicketService.filterList(this.listName);
  }

  ngOnInit(){
    this.ticketListResults = this.multiTicketService.filterList(this.listName);
  }


}
