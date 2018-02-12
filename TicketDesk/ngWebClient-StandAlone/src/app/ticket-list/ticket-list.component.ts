import { Component, OnInit, Input } from '@angular/core';
import { Ticket } from '../models/data';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})


export class TicketListComponent implements OnInit {
  //This will become input
  private headingsList: string[] = ['Title', 'Status', 'Priority', 'Owner', 'Assigned', 'Category', 'Created Date'];
  private displayList: { "ticket": Ticket, "isChecked": boolean }[]
  @Input() ticketListResults: { "ticketList": Ticket[], "maxPages": number };
  @Input() columns: string[];
  selected = new Set();
  isSelected: Map<number, boolean> = new Map;
  currentPage: number;
  //TODO RENAME TICKETLIST THROUGHOUT
  ngOnInit() {
    this.displayList = this.ticketListResults.ticketList.map(
      function (x) { return { "ticket": x, "isChecked": false } }
    )
    this.currentPage = 1;
    for (let ticket of this.ticketListResults.ticketList) {
      this.isSelected.set(ticket.ticketId, false);
    }
  }
  isAllChecked() {
    return this.displayList.every(x => x.isChecked)
  }
  isChecked(ticket: Ticket) {
    return this.isSelected.get(ticket.ticketId)
  }
  selectAll(ev) {
    this.displayList.forEach(x => { x.isChecked = ev.target.checked })
  }

  checkboxSelect(ticket: Ticket) {
    //optimize this with another selection array
    if (this.isSelected.get(ticket.ticketId)) {
      this.selected.delete(ticket)
    } else {
      this.selected.add(ticket)
    }
    this.isSelected.set(ticket.ticketId,
      !this.isSelected.get(ticket.ticketId))
  }


}
