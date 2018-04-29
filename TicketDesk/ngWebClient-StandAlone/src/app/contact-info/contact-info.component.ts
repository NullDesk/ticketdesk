import { UserService } from './../services/user.service';
import { UserDetails } from 'app/models/user-details';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-contact-info',
  templateUrl: './contact-info.component.html',
  styleUrls: ['./contact-info.component.css']
})
export class ContactInfoComponent implements OnInit {
  @Input()
  userName: string;
  user: UserDetails;

  constructor(private contactService: UserService) { }

  ngOnInit() {
    this.populateContactCard();
  }

  private populateContactCard() {
    this.contactService.getAdContactCardInfo(this.userName)
      .subscribe(userDetails => {
        console.warn('user details for contact card', userDetails);
        this.user = userDetails;
      });
  }

}
