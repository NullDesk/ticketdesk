import { Input, Inject, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';


@Component({
  selector: 'app-ticket-detail-editor',
  templateUrl: './ticket-detail-editor.component.html',
  styleUrls: ['./ticket-detail-editor.component.css']
})
export class TicketDetailEditorComponent implements OnInit {
  form: FormGroup;
	displayedSubcategories = [];
	subcategories = {
		"cat": ["hairball", "tail flippy", "sudden death"],
		"dog": ["congenital stupidity", "shedding", "aggression"],
		"frog": ["ew slimy"]
	};
	categories = Object.keys(this.subcategories);
  constructor(@Inject(FormBuilder) fb: FormBuilder) {
    this.form = fb.group({
    	categoryTree: fb.group({
		category: '',
		subcategory: ''
	}),
	    freeEntry: fb.group({title: '', description: ''})
	    
    });
	  this.form.get('categoryTree').get("category").valueChanges.subscribe(
	  (newValue) => {this.displayedSubcategories = this.subcategories[newValue];}
	  );

      
  }
  
	ngOnInit() {
  }

}
