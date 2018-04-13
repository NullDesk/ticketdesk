import { Input, Inject, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { SchemaService, CategoryTree } from '../services/schema.service';
import { SubmitTicketService } from '../services/submit-ticket.service';
import { Ticket, BLANK_TICKET } from '../models/ticket';
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
  subcategories: CategoryTree = {};
  ticketTypes: string[];
  categories: string[];
  buttonText = 'Submit';
  submitting = false;
  constructor(@Inject(FormBuilder) fb: FormBuilder,
    private sts: SubmitTicketService,
    private router: Router,
    private schema: SchemaService) {
    this.form = fb.group(BLANK_TICKET);
    this.form.get('category').valueChanges.subscribe(
      (newValue) => {this.displayedSubcategories = this.subcategories[newValue]; }
    );
  }
  @ViewChild(AttachFileComponent) attachFileComponent: AttachFileComponent;
  ngOnInit() {
    this.form.patchValue(this.initialTicketValue);
    this.schema.getTicketTypes().subscribe(res => this.ticketTypes = res);
    this.schema.getCategoryTree().subscribe(res => {
      this.subcategories = res;
      this.categories = Object.keys(res);
    });
  }

  submit() {
    // do the ticket
    // get back the ID
    this.buttonText = 'Please wait...';
    if (this.submitting) { return; }
    this.submitting = true;
    this.sts.submitTicket(this.form.value).subscribe( res => {
      console.warn('THIS IS WHAT WE GOT BACK', res);
      if (res['ticketID']) {
        this.attachFileComponent.addFile();
        this.router.navigate(['/ticket/' + res['ticketID'].toString()]);
      } else {
        this.router.navigate(['OH_NO_OH_NO_OH_NO']);
      }
      this.submitting = false;
    });
  }
}
