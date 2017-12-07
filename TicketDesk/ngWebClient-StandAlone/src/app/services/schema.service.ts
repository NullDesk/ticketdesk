import { Injectable } from '@angular/core';
import { tickets } from './ticket_db';


@Injectable()
export class SchemaService {
	private types: string[];
	private categoryTree;

  getCategoryTree() {
	return this.categoryTree 
  	
  }

	getTicketTypes() {
  return this.types 
  }
  
	constructor() {
		let categories = Array.from(new Set(tickets.map((tkt) => tkt["category"])));
console.log(categories);
this.types = Array.from( new Set(tickets.map((tkt) => tkt["ticketType"])));
	this.categoryTree = {};
		for (let cat of categories) {
		let subcats = Array.from( new Set(
		 tickets.filter((x) => x["category"] == cat).map((x) => x["subcategory"])
		));
			this.categoryTree[cat] = subcats;
		}
		console.log(this.categoryTree);
	}
	

}
