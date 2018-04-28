import { Input, Output, EventEmitter, Inject, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SchemaService, CategoryTree } from '../services/schema.service';
import { Ticket, BLANK_TICKET } from '../models/ticket';
import { AttachFileComponent } from '../attach-file/attach-file.component';
@Component({
  selector: 'app-ticket-detail-editor',
  templateUrl: './ticket-detail-editor.component.html',
  styleUrls: ['./ticket-detail-editor.component.css'],
  providers: [SchemaService]
})
export class TicketDetailEditorComponent implements OnInit {
  @Input('initialTicketValue') initialTicketValue: Ticket;
  @Output() ticketEmitter = new EventEmitter<any>();
  form: FormGroup;
  displayedSubcategories: string[] = ['Select a category'];
  subcategories: CategoryTree = {};
  ticketTypes: string[];
  priorities: string[];
  categories: string[];
  buttonText = 'Submit';
  submitting = false;
  constructor(
    @Inject(FormBuilder) fb: FormBuilder,
    private schema: SchemaService
  ) {
    this.form = fb.group(BLANK_TICKET);
    this.form.get('category').valueChanges.subscribe(
      (newValue) => {
        this.displayedSubcategories = this.subcategories[newValue];
      }
    );
    this.form.get('category').setValidators([Validators.required, Validators.minLength(1)]);
    this.form.get('subcategory').setValidators([Validators.required, Validators.minLength(1)]);
  }
  @ViewChild(AttachFileComponent) attachFileComponent: AttachFileComponent;
  ngOnInit() {
    this.form.patchValue(this.initialTicketValue);
    this.schema.getTicketTypes().subscribe(res => this.ticketTypes = res);
    this.schema.getPriorities().subscribe(res => this.priorities = res);
    this.schema.getCategoryTree().subscribe(res => {
      this.subcategories = res;
      this.categories = Object.keys(res);
      this.displayedSubcategories = this.subcategories[this.form.get('category').value];
    });
  }

  ticketEmit() {
    if (this.form.invalid) {
      console.log('this should not have happened');
      return;
    }
    this.buttonText = 'Please wait...';
    if (this.submitting) { return; }
    this.submitting = true;
    this.ticketEmitter.emit(this.form.value);
  }
}
