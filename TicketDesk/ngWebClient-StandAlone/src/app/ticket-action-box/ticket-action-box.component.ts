import { Component, Input, OnInit } from '@angular/core';
import { Ticket } from '../models/data';
import { TicketActionEnum } from '../models/ticket-actions.constants';
import { OnChanges, SimpleChanges } from '@angular/core/src/metadata/lifecycle_hooks';

@Component({
  selector: 'app-ticket-action-box',
  templateUrl: './ticket-action-box.component.html',
  styleUrls: ['./ticket-action-box.component.css']
})
export class TicketActionBoxComponent implements OnInit, OnChanges {
  @Input() ticket: Ticket;
  @Input() permissions: number;
  allowedActions: TicketActionEnum[];
  activeAction: TicketActionEnum = null;
  detailEditorNeeded = false;
  public isCollapsed = true;
  commentPlaceholder = 'Comment';

  setActiveAction(action: TicketActionEnum) {
    if (action === this.activeAction) { return false; }
    this.activeAction = action;
    this.detailEditorNeeded = this.activeAction === TicketActionEnum.EDITTICKET;
    this.commentPlaceholder = action.requiresComment ? 'Comment (required)' : 'Comment (optional)';
    return true;

  }

  constructor() {}

  ngOnInit() {
    console.warn("on init permissions", this.permissions);
    this.allowedActions = TicketActionEnum.getActivityList(this.permissions);
    console.warn("ticket box says permissions are", this.allowedActions)
  }

  ngOnChanges(changes: SimpleChanges) {
    console.warn("changes called!", changes)
    if (changes['permissions']) {
      console.log("changed")
      this.permissions = changes['permissions'].currentValue
      this.allowedActions = TicketActionEnum.getActivityList(this.permissions);
    }
  }

}
