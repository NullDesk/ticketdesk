import { User_Details } from 'app/models/user';
import { Component, OnInit, Input } from '@angular/core';
import { AdUserComponent } from '../ad-user/ad-user.component';

@Component({
  selector: 'app-contact-info',
  templateUrl: './contact-info.component.html',
  styleUrls: ['./contact-info.component.css']
})
export class ContactInfoComponent implements OnInit {
  @Input() user: User_Details
  constructor() { }

  ngOnInit() { }

}
