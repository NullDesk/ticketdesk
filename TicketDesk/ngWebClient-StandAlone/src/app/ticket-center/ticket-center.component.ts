import { Injectable, Component, OnInit } from '@angular/core';
import { Ticket } from '../models/data';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-ticket-center',
  templateUrl: './ticket-center.component.html',
  styleUrls: ['./ticket-center.component.css']
})


export class TicketCenterComponent implements OnInit {
  tabNames: string[] = ['Open', 'Assigned', 'All', 'Submitted', 'Closed']; // Make input settings at some point

  ngOnInit() {
  }

}
