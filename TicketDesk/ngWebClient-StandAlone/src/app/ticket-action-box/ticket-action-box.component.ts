import { Component, Input, OnInit } from '@angular/core';
import { Ticket } from '../models/ticket';
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
    this.allowedActions = TicketActionEnum.getActivityList(this.permissions);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['permissions']) {
      this.permissions = changes['permissions'].currentValue;
      console.warn('permissions now', this.permissions);
      this.allowedActions = TicketActionEnum.getActivityList(this.permissions);
    }
  }

}
