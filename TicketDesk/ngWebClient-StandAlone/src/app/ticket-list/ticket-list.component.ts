import { Component, OnInit, Input } from '@angular/core';
import { Ticket } from '../models/data';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})

export class TicketListComponent implements OnInit {
  // This will become input
  private headingsList: string[] = ['Title', 'Status', 'Priority', 'Owner', 'Assigned', 'Category', 'Created Date'];
  // Adds a vairable to add keep track of checkbox
  private displayList: {'ticket': Ticket, 'checked': boolean}[];
  @Input() ticketListResults: { 'ticketList': Ticket[], 'maxPages': number };
  @Input() columns: string[];
  currentPage: number;
  ngOnInit() {
    this.displayList = [];
    // filter removes objects not of type ticket or null/undefined
    for (const ticket of this.ticketListResults.ticketList.filter( x => x)) {
        this.displayList.push({'ticket': ticket, 'checked': false});
    }
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
