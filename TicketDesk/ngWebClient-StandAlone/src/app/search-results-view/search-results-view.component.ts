import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { MultiTicketService } from '../services/multi-ticket.service';
import { Ticket } from '../models/ticket';

@Component({
  selector: 'app-search-results-view',
  templateUrl: './search-results-view.component.html',
  styleUrls: ['./search-results-view.component.css']
})

export class SearchResultsViewComponent implements OnInit {
  private term: string;
  private ticketListResults: { 'ticketList': Ticket[], 'maxPages': number };

  constructor(private multiTicketService: MultiTicketService,
    private activatedRoute: ActivatedRoute) {
    this.activatedRoute.params.subscribe(params => {
      this.term = params['term'];
    });
  }

  ngOnInit() {
    this.ticketListResults = this.multiTicketService.filterList(this.term);
  }

}
