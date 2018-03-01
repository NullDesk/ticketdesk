import { Injectable } from '@angular/core';
import { single_user } from './user_db';
import { Users } from './user_db';
import { UserDetails } from 'app/models/user';

@Injectable()
export class AdContactService {

  constructor() { }
  GetUser()
  {
    return single_user;
  }

  GetContactCardInfo(userID: string)
  {
    let SingleUser: UserDetails = null;
    for (const user of Users){
      if (user.u_id == userID){
        SingleUser = user;
        break;
      }
    }
    return SingleUser;
  }
}
