import { Component, OnInit, Input } from '@angular/core';
import { Output } from '@angular/core/src/metadata/directives';
import { EventEmitter } from '@angular/core/src/event_emitter';
import { TicketPreview } from 'app/models/ticket-preview';

@Component({
  selector: 'app-ticket-list-row',
  templateUrl: './ticket-list-row.component.html',
  styleUrls: ['./ticket-list-row.component.css']
})
export class TicketListRowComponent implements OnInit {
  @Input() ticket: TicketPreview;
  //@Output() onRowClick = new EventEmitter<TicketPreview>(); //TODO implement this in HTML Will be more complex for selecting and going to ticket

  ngOnInit() {
  }

}
