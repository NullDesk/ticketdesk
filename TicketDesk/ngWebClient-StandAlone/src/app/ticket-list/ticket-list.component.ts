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
  @Input() ticketListResults: { 'ticketList': Ticket[], 'maxPages': number };
  @Input() columns: string[];
  currentPage: number;
  ngOnInit() {
    // State is created from ngModal in Angular
    this.ticketListResults.ticketList.forEach(x => x.state = false);
    this.currentPage = 1;
  }
  isAllChecked() {
    return this.ticketListResults.ticketList.every(x => x.state);
  }
  selectAll(ev) {
    this.ticketListResults.ticketList.forEach(x => { x.state = ev.target.checked; });
  }
  getSelected() {
    return this.ticketListResults.ticketList.filter( x => x.state);
  }
}
