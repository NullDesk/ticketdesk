import { Component, OnInit, Input} from '@angular/core';
import { Ticket } from '../models/data';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})


export class TicketListComponent implements OnInit {
  //This will become input
  ticketList: Ticket[];
  headingsList: string [] = ['Title', 'Status', 'Priority', 'Owner', 'Assigned', 'Category', 'Created Date'];
  @Input() ticketListResults: {"ticketList": Ticket[], "maxPages": number};
  @Input() columns: string[];
  selected = [];
  currentPage:number;
  ngOnInit() {
    this.currentPage = 1;
    this.ticketList = this.ticketListResults.ticketList
  }

  onSelect({selected}) {
    this.selected.push(...selected)
  }
  

}
