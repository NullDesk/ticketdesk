import { Injectable } from '@angular/core';
import { single_user } from '../services/user_db';

@Injectable()
export class AdContactService {

  constructor() { };
  GetUser()
  {
    return single_user;
  }
}
