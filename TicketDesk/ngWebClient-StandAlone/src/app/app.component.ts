import { Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormControl } from '@angular/forms';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'TITLE TKTKTKTKTK';
  navbarCollapsed = true;
  searchBox: FormControl;	

	constructor() {
	this.searchBox = new FormControl('');
	}
}
