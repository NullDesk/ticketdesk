import { Input, Inject, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { SubmitTicketService } from '../services/submit-ticket.service';
import {Ticket, BLANK_TICKET} from '../models/data';
import {AttachFileComponent } from '../attach-file/attach-file.component';
@Component({
  selector: 'app-ticket-detail-editor',
  templateUrl: './ticket-detail-editor.component.html',
  styleUrls: ['./ticket-detail-editor.component.css'],
  providers: [SubmitTicketService]
})
export class TicketDetailEditorComponent implements OnInit {
  form: FormGroup;
	displayedSubcategories: string[] = ["Select a category"];
	subcategories = {
		"cat": ["hairball", "tail flippy", "sudden death"],
		"dog": ["congenital stupidity", "shedding", "aggression"],
		"frog": ["ew slimy"]
	};
	categories = Object.keys(this.subcategories);
  constructor(@Inject(FormBuilder) fb: FormBuilder, private sts: SubmitTicketService, private router: Router) {
	  this.form = fb.group(
	    BLANK_TICKET);
	  this.form.get("category").valueChanges.subscribe(
	  (newValue) => {this.displayedSubcategories = this.subcategories[newValue];}
	  );

      
  };
	@Input('initialTicketValue') tkt: Ticket;
 	@ViewChild(AttachFileComponent) attachFileComponent: AttachFileComponent;
	ngOnInit() {
	}

	submit(){
		// do the ticket
		// get back the ID
		let ticketId = this.sts.submitTicket(this.form.value);
		this.attachFileComponent.addFile();
		this.router.navigate(['/ticket/' + ticketId.toString()]);
	}
}
