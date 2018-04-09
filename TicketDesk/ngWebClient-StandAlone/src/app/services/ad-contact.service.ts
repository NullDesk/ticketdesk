import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Users } from './user_db';
import { UserDetails } from 'app/models/user-details';
import { catchError, retry } from 'rxjs/operators';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import * as settings from '../app-settings';

@Injectable()
export class AdContactService {

  constructor() { }

  getContactCardInfo(userID: string): Observable<UserDetails> {
    return Observable.of({firstName: 'Juan', lastName: 'Perez', phoneNumber: '(503)555-1515', email: 'jjpe@wherever.com', userId: userID});
  }
}
