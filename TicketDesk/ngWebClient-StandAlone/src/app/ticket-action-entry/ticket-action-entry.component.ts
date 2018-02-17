import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { TicketActionEnum } from '../models/ticket-actions.constants'; 

@Component({
  selector: 'app-ticket-action-entry',
  templateUrl: './ticket-action-entry.component.html',
  styleUrls: ['./ticket-action-entry.component.css']
})
export class TicketActionEntryComponent implements OnInit {
	@Input()
	commentPlaceholder: string;
	constructor() { }
  
	ngOnInit() {
  }
	
	submit() {
	console.log("you made a click");
	}
  

}
