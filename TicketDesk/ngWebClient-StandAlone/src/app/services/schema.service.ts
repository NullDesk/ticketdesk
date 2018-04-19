import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/publishReplay';
import * as settings from '../app-settings';

export interface CategoryTree {
  [key: string]: string[];
}

interface ListWrapper {
  'list': string[];
}

@Injectable()
export class SchemaService {
  // https://gist.github.com/Armenvardanyan95/eb8a078261eec2e4ff8c87bd8d7820a4#file-cached-response-ts
  private categories: Observable<CategoryTree> =  this.http
    .get<CategoryTree>(settings.categoriesURL)
    .publishReplay(1)
    .refCount();

  private types: Observable<string[]> = this.http
    .get<ListWrapper>(settings.ticketTypesURL)
    .map(res => res['list'])
    .publishReplay(1)
    .refCount();

  private priorities: Observable<string[]> = this.http
    .get<ListWrapper>(settings.prioritiesURL)
    .map(res => res['list'])
    .publishReplay(1)
    .refCount();

  public getCategoryTree(): Observable<CategoryTree> {
    return this.categories;
  }

  public getTicketTypes(): Observable<string[]> {
    return this.types;
  }

  public getPriorities(): Observable<string[]> {
    return this.priorities;
  }

  constructor(private http: HttpClient) {
  }


}
