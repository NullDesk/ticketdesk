import { Component, OnInit, Input } from '@angular/core';
import { ListTicket, columnHeadings } from '../models/list-ticket';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})

export class TicketListComponent implements OnInit {
  // imported into the class, so can be used in HTML
  private colHeadings = columnHeadings;
  // Adds a vairable to add keep track of checkbox
  private displayList: {ticket: ListTicket, checked: boolean}[];
  @Input() ticketListResults: { ticketList: ListTicket[], maxPages: number };
  @Input() columns: string[];
  currentPage: number;

  ngOnInit() {
    // filter removes objects not of type ticket or null/undefined
    this.displayList = this.ticketListResults.ticketList
          .filter( x => x)
          .map(ticket => ({ticket: ticket, checked: false}));
    this.currentPage = 1;
}

  isAllChecked() {
    return this.displayList.every(x => x.checked);
  }

  selectAll(ev) {
    this.displayList.forEach(x => {x.checked = ev.target.checked; });
  }

  getSelected() {
    return this.displayList.filter( x => x.checked);
  }
}
