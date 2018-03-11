import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
@Component({
  selector: 'app-search-box',
  templateUrl: './search-box.component.html',
  styleUrls: ['./search-box.component.css']
})
export class SearchBoxComponent implements OnInit {

  searchBox: FormControl;	

	constructor() {
	this.searchBox = new FormControl('');
	}

  ngOnInit() {
  }

}
