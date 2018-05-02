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
  private ticketList: TicketStub[];
  private listReady: Boolean = false;
  constructor(private searchService: SearchService,
    private activatedRoute: ActivatedRoute) {
    this.activatedRoute.params.subscribe(params => {
      this.term = params['term'];
      this.search(this.term);
    });
  }

  ngOnInit() {}

  search(term: string): void {
    this.listReady = false;
    console.log('searching for', term);
    this.searchService.search(term)
        .subscribe(ticketList => {
          this.ticketList = ticketList;
          this.listReady = true;
        });
  }
}
