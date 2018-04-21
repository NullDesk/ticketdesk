import { UserDetails } from 'app/models/user';
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
    console.warn('URL:', settings.adUserURL + userName);
    return this.http.get<UserDetails>(settings.adUserURL + userName)
      .pipe(catchError(this.handleError));
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
