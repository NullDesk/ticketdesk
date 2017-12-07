import { Injectable, Component, OnInit } from '@angular/core';
import { Ticket } from '../models/data';
import { MultiTicketService } from '../services/multi-ticket.service';
import {Router, ActivatedRoute, Params} from '@angular/router';
import { tickets } from '../services/ticket_db';

@Component({
  selector: 'app-ticket-center',
  templateUrl: './ticket-center.component.html',
  styleUrls: ['./ticket-center.component.css']
})


export class TicketCenterComponent implements OnInit {
  tabNames: string[] = ["Open", "Assigned", "All", "Submitted", "Closed" ] //MAKE input/ settings at some point
  ticketList: {"list":Ticket[], "maxPages" : number};
  listName: string;
  constructor(private multiTicketService : MultiTicketService) { 
    
  }
  setListName(tabName : string){
    this.listName = tabName;
    this.ticketList = this.multiTicketService.filterList(this.listName);
  }

  ngOnInit() { 
    this.ticketList = this.multiTicketService.filterList(this.listName);
  }

}
