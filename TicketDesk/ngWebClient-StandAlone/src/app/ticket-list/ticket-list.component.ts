import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TicketStub, columnHeadings } from '../models/ticket-stub';
import { FormsModule } from '@angular/forms';
import { getTicketStatusText } from '../models/ticket';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})

export class TicketListComponent implements OnInit {
  // imported into the class, so can be used in HTML
  private colHeadings = columnHeadings;
  private getStatusText = getTicketStatusText;
  // Adds a vairable to add keep track of checkbox
  private displayList: {ticket: TicketStub, checked: boolean}[];
  @Input() ticketList:  TicketStub[];
  @Input() pagination: {current: number, max: number } = null;
  @Input() columns: string[];
  @Output() pageChange = new EventEmitter<number>();

  ngOnInit() {
    // filter removes objects not of type ticket or null/undefined
    this.displayList = this.ticketList
          .filter( x => x)
          .map(ticket => ({ticket: ticket, checked: false}));
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

  onPageChange(page: number) {
    this.pageChange.emit(page);
  }

}
