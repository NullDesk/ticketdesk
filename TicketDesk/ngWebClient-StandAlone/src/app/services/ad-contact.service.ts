import { UserDetails } from './../models/user-details';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { catchError, retry } from 'rxjs/operators';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import * as settings from '../app-settings';

@Injectable()
export class AdContactService {
  constructor(private http: HttpClient) { }

  getContactCardInfo(userName: string): Observable<UserDetails> {
    return this.http.get(settings.adUserURL + userName)
      .pipe(catchError(this.handleError));
  }

  formatUserDetails(
    userDetails: Object): UserDetails {
    console.warn('userDetails:', userDetails)  
    return Object.assign({}, {
      firstName: 'name',
      lastName: '<Last Name>',
      phoneNumber: '<Phone Number>',
      email: 'stillneed@activedirectory.com',
      userId: ''
    });

  }

  private handleError(error: HttpErrorResponse): ErrorObservable {
    if (error.error instanceof ErrorEvent) {
      // ... this is a client side error, handle it!
      console.error(`client error occurred: ${error.error.message}`);
    } else {
      // ... this is a server error!
      console.error(`server error occurred, status code ${error.status}`);
    }
    return new ErrorObservable('Experiencing some issues, we are sorry');
  }
}
