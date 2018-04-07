import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-search-box',
  templateUrl: './search-box.component.html',
  styleUrls: ['./search-box.component.css']
})
export class SearchBoxComponent implements OnInit {

  searchBox: FormControl;

  constructor(private router: Router) {
    this.searchBox = new FormControl('');
  }
  goToSearchPage() {
    this.router.navigate(['search', this.searchBox.value]);
  }
  ngOnInit() {
  }

}
