import { Component, OnInit } from '@angular/core';
import { UserDetails } from 'app/models/user';
import { single_user } from 'app/services/user_db';

@Component({
  selector: 'app-ad-user',
  templateUrl: './ad-user.component.html',
  styleUrls: ['./ad-user.component.css']
})
export class AdUserComponent implements OnInit {
  user: UserDetails;
  constructor() {
    this.user = single_user;
  }

  ngOnInit() {
  }

}
