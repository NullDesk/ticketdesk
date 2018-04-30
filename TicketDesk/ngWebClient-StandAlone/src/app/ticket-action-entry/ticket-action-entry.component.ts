import { Component, Input, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { TicketActionEnum } from '../models/ticket-actions.constants';
import { AdUserSelectorComponent } from '../ad-user-selector/ad-user-selector.component';
import { OnChanges, SimpleChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { SingleTicketService } from '../services/single-ticket.service';
import { RedirectService } from '../services/redirect.service';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-ticket-action-entry',
  templateUrl: './ticket-action-entry.component.html',
  styleUrls: ['./ticket-action-entry.component.css'],
})
export class TicketActionEntryComponent implements OnInit, OnChanges {
  @Input()
  commentPlaceholder: string;
  @Input()
  action: TicketActionEnum;
  ticketActionForm: FormGroup;
  fb: FormBuilder;
  ticketId: number = null;
  constructor(@Inject(FormBuilder) fb: FormBuilder,
    private redirectService: RedirectService,
    private singleTicketService: SingleTicketService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    this.activatedRoute.params.subscribe(params => {
      this.ticketId = Number(params['ticketID']);
    });
    this.fb  = fb;
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['action'] && changes['action'].currentValue) {
      this.ticketActionForm = this.fb
        .group(changes.action.currentValue.formTemplate);
    }
  }

  submitAction() {
    const formValue = this.ticketActionForm.value;
    formValue.ticketId = this.ticketId;
    console.log(formValue);
    this.singleTicketService.submitTicketAction(formValue, this.action).subscribe(
      res => {
          this.redirectService.requestTabChange();
      }
    );
  }

}
