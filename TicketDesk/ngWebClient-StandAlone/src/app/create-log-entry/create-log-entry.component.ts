import { Input, Inject, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';


@Component({
  selector: 'app-create-log-entry',
  templateUrl: './create-log-entry.component.html',
  styleUrls: ['./create-log-entry.component.css']
})
export class CreateLogEntryComponent implements OnInit {
  form: FormGroup;
  constructor(@Inject(FormBuilder) fb: FormBuilder) {
    this.form = fb.group({
			freeEntry: fb.group({description: ''})
    });
  }
  ngOnInit() {
  }
}
