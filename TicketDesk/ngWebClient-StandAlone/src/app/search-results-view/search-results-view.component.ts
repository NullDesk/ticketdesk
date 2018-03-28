import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Ticket } from '../models/ticket';
import { SearchService} from '../services/search.service';

@Component({
  selector: 'app-search-results-view',
  templateUrl: './search-results-view.component.html',
  styleUrls: ['./search-results-view.component.css']
})

export class SearchResultsViewComponent implements OnInit {
  private term: string;
  private ticketListResults: { 'ticketList': Ticket[], 'maxPages': number };

  constructor(private searchService: SearchService,
    private activatedRoute: ActivatedRoute) {
    this.activatedRoute.params.subscribe(params => {
      this.term = params['term'];
    });
  }

  ngOnInit() {
    this.ticketListResults = this.searchService.search(this.term);
  }

}
