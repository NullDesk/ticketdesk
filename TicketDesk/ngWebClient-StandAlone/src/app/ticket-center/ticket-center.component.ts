import { Injectable, Component, OnInit } from '@angular/core';
import { Ticket } from '../models/ticket';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-ticket-center',
  templateUrl: './ticket-center.component.html',
  styleUrls: ['./ticket-center.component.css']
})


export class TicketCenterComponent implements OnInit {
  tabNames: string[] = ['unassigned', 'assignedToMe', 'mytickets', 'opentickets', 'historytickets']; // Make input settings at some point

  ngOnInit() {
  }

}
