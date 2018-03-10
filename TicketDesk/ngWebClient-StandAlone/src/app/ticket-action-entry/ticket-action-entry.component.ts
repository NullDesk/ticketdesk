import { Component, Input, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { TicketActionEnum } from '../models/ticket-actions.constants';

@Component({
  selector: 'app-ticket-action-entry',
  templateUrl: './ticket-action-entry.component.html',
  styleUrls: ['./ticket-action-entry.component.css']
})
export class TicketActionEntryComponent implements OnInit {
  @Input()
  commentPlaceholder: string;
  @Input()
  action: TicketActionEnum;
  ticketActionForm: FormGroup;
  fb: FormBuilder;

  constructor(@Inject(FormBuilder) fb: FormBuilder) {
    this.fb  = fb;
  }

  ngOnInit() {
    this.ticketActionForm = this.fb.group(this.action.formTemplate);
  }

  submit() {
    console.log('look at this action');
    console.log(this.action);
    console.log('you made a click');
  }


}
