import { Component, Input, OnInit } from '@angular/core';
import { TicketActionEnum } from '../models/ticket-actions.constants';

@Component({
  selector: 'app-ticket-action-box',
  templateUrl: './ticket-action-box.component.html',
  styleUrls: ['./ticket-action-box.component.css']
})
export class TicketActionBoxComponent implements OnInit {
	allowedActions: TicketActionEnum[];
	activeAction: TicketActionEnum = null;
	commentPlaceholder: string = "Comment";	
  
	setActiveAction(action: TicketActionEnum) {
	  if (action == this.activeAction) { return false; }
	  this.activeAction = action;
	  this.commentPlaceholder = action.requiresComment ? "Comment (required)" : "Comment (optional)";  
	  return true;

  }

  @Input() permissions: number;
  constructor() { }

  ngOnInit() {
	this.allowedActions = TicketActionEnum.getActivityList(this.permissions);
  }

}
