import { Component, OnInit } from '@angular/core';
import { User_Details } from 'app/models/user';
import { single_user } from 'app/services/user_db';

@Component({
  selector: 'app-ad-user',
  templateUrl: './ad-user.component.html',
  styleUrls: ['./ad-user.component.css']
})
export class AdUserComponent implements OnInit {
  user: User_Details;
  constructor() 
  { 
    this.user = single_user;
  }

  ngOnInit() {
  }

}
