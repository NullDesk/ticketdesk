import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { SearchService} from '../services/search.service';
import { TicketStub } from '../models/ticket-stub';


@Component({
  selector: 'app-search-results-view',
  templateUrl: './search-results-view.component.html',
  styleUrls: ['./search-results-view.component.css']
})

export class SearchResultsViewComponent implements OnInit {
  private term: string;
  private ticketListResults: { ticketList: TicketStub[], maxPages: number } = { ticketList: undefined, maxPages: null};
  private listReady: Boolean = false;
  constructor(private searchService: SearchService,
    private activatedRoute: ActivatedRoute) {
    this.activatedRoute.params.subscribe(params => {
      this.term = params['term'];
    });
  }

  ngOnInit() {
    this.getTicketList('');
  }

  getTicketList(listName: string): void {
    this.listReady = false;
    console.log('Getting ticketlist for', listName);
    this.searchService.search(listName)
        .subscribe(ticketList => {
          this.ticketListResults.ticketList = ticketList;
          this.listReady = true;
        });
  }
}
