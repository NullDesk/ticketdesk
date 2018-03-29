import { Injectable } from '@angular/core';
import { Users } from './user_db';
import { UserDetails } from 'app/models/user-details';

@Injectable()
export class AdContactService {

  constructor() { }

  getContactCardInfo(userID: string): UserDetails {
    for (const user of Users) {
      if (user.u_id === userID) {
        return user; 
      }
    }
    return null;
  }
}
