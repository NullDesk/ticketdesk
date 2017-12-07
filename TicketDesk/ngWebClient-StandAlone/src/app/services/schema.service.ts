import { Injectable } from '@angular/core';

@Injectable()
export class SchemaService {
  getCategoryTree() {
	return {
		"cat": ["hairball", "tail flippy", "sudden death"],
		"dog": ["congenital stupidity", "shedding", "aggression"],
		"frog": ["ew slimy"]
	};
  	
  }

	getTicketTypes() {
  return ["Problem", "Big Problem", "Explosion"];
  }
  
	constructor() { }

}
