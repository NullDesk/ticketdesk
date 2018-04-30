import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class RedirectService {

  private tabChangeRequester = new Subject<boolean>();
  tabChangeRequested = this.tabChangeRequester.asObservable();

  constructor() { }

  requestTabChange() {
    this.tabChangeRequester.next(true);
  }
}
