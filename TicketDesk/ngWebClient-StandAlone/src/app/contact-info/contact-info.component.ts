import { AdContactService } from './../services/ad-contact.service';
import { UserDetails } from 'app/models/user-details';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-contact-info',
  templateUrl: './contact-info.component.html',
  styleUrls: ['./contact-info.component.css']
})
export class ContactInfoComponent implements OnInit {
  @Input() ownerId: string;
  user: UserDetails;
  constructor(private contactService: AdContactService) {
    this.contactService = contactService;
  }

  ngOnInit() {
    this.contactService.getContactCardInfo(this.ownerId)
      .subscribe(res => {
        this.user = res;
      });
  }

}
