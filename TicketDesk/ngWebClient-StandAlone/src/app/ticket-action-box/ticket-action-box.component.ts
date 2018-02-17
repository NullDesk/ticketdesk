import { Component, Input, OnInit } from '@angular/core';
import { Ticket } from '../models/data';
import { TicketActionEnum } from '../models/ticket-actions.constants';

@Component({
  selector: 'app-ticket-action-box',
  templateUrl: './ticket-action-box.component.html',
  styleUrls: ['./ticket-action-box.component.css']
})
export class TicketActionBoxComponent implements OnInit {
	@Input() ticket: Ticket;
	allowedActions: TicketActionEnum[];
	activeAction: TicketActionEnum = null;
	detailEditorNeeded: boolean = false;
	commentPlaceholder: string = "Comment";	
  
	setActiveAction(action: TicketActionEnum) {
		if (action == this.activeAction) { return false; }
	  this.activeAction = action;
		this.detailEditorNeeded = this.activeAction == TicketActionEnum.EDITTICKET
		this.commentPlaceholder = action.requiresComment ? "Comment (required)" : "Comment (optional)";  
		return true;

  }

  @Input() permissions: number;
  constructor() {}

  ngOnInit() {
	this.allowedActions = TicketActionEnum.getActivityList(this.permissions);
  }

}
