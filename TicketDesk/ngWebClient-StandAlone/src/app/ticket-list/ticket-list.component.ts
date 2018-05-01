import { Component, OnInit, Input, Output,
         EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { TicketStub, columnHeadings } from '../models/ticket-stub';
import { FormsModule } from '@angular/forms';
import { getTicketStatusText } from '../models/ticket';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})

export class TicketListComponent implements OnInit, OnChanges {
  // imported into the class, so can be used in HTML
  private colHeadings = columnHeadings;
  private getStatusText = getTicketStatusText;
  // Adds a vairable to add keep track of checkbox
  private displayList: {ticket: TicketStub, checked: boolean}[];
  @Input() notSortable: Boolean;
  @Input() ticketList:  TicketStub[];
  @Input() pagination: {current: number, max: number } = null;
  @Output() pageChange = new EventEmitter<number>();
  @Output() sortTrigger = new EventEmitter<string>();

  ngOnInit() {
    if (this.notSortable) {
      this.colHeadings = columnHeadings.map(x => {
        x.direction = 'false';
        return x;
      });
    } else {
      this.colHeadings = columnHeadings.map(x => Object.assign({}, x));
    }
    this.makeDisplayList(this.ticketList);
  }

  makeDisplayList(ticketList: TicketStub[]) {
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

  ngOnChanges(changes: SimpleChanges) {
    for (const propName of Object.keys(changes)) {
      const change = changes[propName];
      console.log('ngChange triggered new value: ', JSON.stringify(change.currentValue));
      if (propName === 'ticketList') {
        this.makeDisplayList(change.currentValue);
      }
      if (propName === 'pagination') {
        this.pagination = change.currentValue;
      }
    }
  }

  headerSort(colHeading: {header: string, direction: string} ) {
    if (colHeading.direction !== 'false') {
      const colName = colHeading.header.replace(/\s/g, '');
      // Cycle through all the sort options
      if (colHeading.direction === 'sortable' || colHeading.direction === 'desc') {
        colHeading.direction = 'asc';
      } else {
        colHeading.direction = 'desc';
      }
      this.sortTrigger.emit(colName);
    }
  }

  resetSort(colHeading: {header: string, direction: string}) {
    if (colHeading.direction !== 'false') {
      this.sortTrigger.emit('reset');
    }
  }
}
