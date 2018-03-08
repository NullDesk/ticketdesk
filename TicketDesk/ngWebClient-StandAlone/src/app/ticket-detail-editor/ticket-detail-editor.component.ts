import { Input, Inject, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { SchemaService } from '../services/schema.service';
import { SubmitTicketService } from '../services/submit-ticket.service';
import { Ticket, BLANK_TICKET } from '../models/data';
import { AttachFileComponent } from '../attach-file/attach-file.component';
@Component({
  selector: 'app-ticket-detail-editor',
  templateUrl: './ticket-detail-editor.component.html',
  styleUrls: ['./ticket-detail-editor.component.css'],
  providers: [SubmitTicketService, SchemaService]
})
export class TicketDetailEditorComponent implements OnInit {
  @Input('initialTicketValue') initialTicketValue: Ticket;
  form: FormGroup;
  displayedSubcategories: string[] = ['Select a category'];
  subcategories: Object = {};
  ticketTypes: string[];
  categories: string[];
  constructor(@Inject(FormBuilder) fb: FormBuilder,
    private sts: SubmitTicketService,
    private router: Router,
    private schema: SchemaService) {
    this.subcategories = schema.getCategoryTree();
    this.categories = Object.keys(this.subcategories);
    this.ticketTypes = schema.getTicketTypes();
    this.form = fb.group(BLANK_TICKET);
    this.form.get('category').valueChanges.subscribe(
      (newValue) => {this.displayedSubcategories = this.subcategories[newValue]; }
    );
  }
  @ViewChild(AttachFileComponent) attachFileComponent: AttachFileComponent;
  ngOnInit() {
    this.form.patchValue(this.initialTicketValue);
  }

  submit() {
    // do the ticket
    // get back the ID
    const ticketId = this.sts.submitTicket(this.form.value);
    this.attachFileComponent.addFile();
    this.router.navigate(['/ticket/' + ticketId.toString()]);
  }
}
