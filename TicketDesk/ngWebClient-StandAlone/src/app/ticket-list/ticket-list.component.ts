import { Component, OnInit, Input} from '@angular/core';
import { Ticket } from '../models/data';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})


export class TicketListComponent implements OnInit {
  //This will become input
  @Input() ticketList: Ticket[];
  selected = [];
  ngOnInit() {
    
  }

  onSelect({selected}) {
    this.selected.push(...selected)
  }
  

}
