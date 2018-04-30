import { Injectable } from '@angular/core';
import { Subject }    from 'rxjs/Subject';

@Injectable()
export class RedirectService {

  private tabChangeRequester = new Subject<boolean>();
  tabChangeRequested = this.tabChangeRequested.asObservable();

  constructor() { }

  requestTabChange() {
    console.log("requestTabChange called");
    this.tabChangeRequester.next(true);
  }
}
