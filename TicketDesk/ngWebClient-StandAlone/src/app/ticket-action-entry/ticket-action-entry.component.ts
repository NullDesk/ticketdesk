import { Component, Input, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { TicketActionEnum } from '../models/ticket-actions.constants';
import { AdUserSelectorComponent } from '../ad-user-selector/ad-user-selector.component';
import { OnChanges, SimpleChanges } from '@angular/core/src/metadata/lifecycle_hooks';

@Component({
  selector: 'app-ticket-action-entry',
  templateUrl: './ticket-action-entry.component.html',
  styleUrls: ['./ticket-action-entry.component.css']
})
export class TicketActionEntryComponent implements OnInit, OnChanges {
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
  }
  ngOnChanges(changes: SimpleChanges) {
    if (changes['action'] && changes['action'].currentValue) {
      console.warn('this is the cahnge for the action entry', changes);
      this.ticketActionForm = this.fb.group(changes.action.currentValue.formTemplate);
    }
  }

  submit() {
    console.log('you made a click');
  }


}
