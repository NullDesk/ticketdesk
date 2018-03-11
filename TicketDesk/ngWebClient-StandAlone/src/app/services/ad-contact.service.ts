import { Injectable } from '@angular/core';
import { Users } from './user_db';
import { UserDetails } from 'app/models/user-details';

@Injectable()
export class AdContactService {

  constructor() { }
  getUser() {
    return Users[0];
  }

  getContactCardInfo(userID : string) {
    let singleUser: UserDetails = null;
    for(let user of Users){
      if(user.userId == userID){
        singleUser = user;
        break;
      }
    }
    return singleUser;
  }
}
