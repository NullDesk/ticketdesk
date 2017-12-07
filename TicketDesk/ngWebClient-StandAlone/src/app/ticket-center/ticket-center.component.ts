import { Injectable, Component, OnInit } from '@angular/core';
import { Ticket } from '../models/data';
import { MultiTicketService } from '../services/multi-ticket.service';
import {Router, ActivatedRoute, Params} from '@angular/router';

@Component({
  selector: 'app-ticket-center',
  templateUrl: './ticket-center.component.html',
  styleUrls: ['./ticket-center.component.css']
})
export class TicketCenterComponent implements OnInit {
  ticketList: Ticket[];
  listName: string = "all";
  constructor(private multiTicketService : MultiTicketService) { 
    
  }

  ngOnInit() { 
    this.ticketList = this.multiTicketService.FilterList("all")
  }

}
