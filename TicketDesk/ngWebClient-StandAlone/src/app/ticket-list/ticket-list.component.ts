import { Component, OnInit, Input} from '@angular/core';
import { Ticket } from '../models/data';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})


export class TicketListComponent implements OnInit {
  //This will become input
  headingsList: string [] = ['Title', 'Status', 'Priority', 'Owner', 'Assigned', 'Category', 'Created Date'];
  @Input() ticketListResults: {"ticketList": Ticket[], "maxPages": number};
  @Input() columns: string[];
  selected = new Set();
  isSelected: Map<number, boolean> = new Map;
  currentPage:number;
  ngOnInit() {
    this.currentPage = 1;
    for (let ticket of this.ticketListResults.ticketList){
      this.isSelected.set(ticket.ticketId, false);
    }
  }
  checkAll(){
    return (this.selected.size != 0)
  }
  isChecked(ticket:Ticket){
    return this.isSelected.get(ticket.ticketId)
  }
  selectAll(){
    if (this.selected.size == 0){
      this.selected = new Set(this.ticketListResults.ticketList)
    }else {
      for (let ticket of this.ticketListResults.ticketList){
        this.isSelected.set(ticket.ticketId, false);
      }
      this.selected = new Set();
    }
  }

  checkboxSelect(ticket:Ticket) {
    //optimize this with another selection array
    if (this.isSelected.get(ticket.ticketId)){
      this.selected.delete(ticket)
    }else {
      this.selected.add(ticket)
    }
    this.isSelected.set(ticket.ticketId,
              !this.isSelected.get(ticket.ticketId))
  }
  

}
