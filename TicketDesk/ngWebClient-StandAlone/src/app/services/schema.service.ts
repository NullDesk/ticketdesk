import { Injectable } from '@angular/core';
import { tickets } from './ticket_db';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/publishReplay';
import * as settings from '../app-settings';

interface Schema {
  categoryTree: CategoryTree;
  typeList: string[];
}

interface CategoryTree {
  [key: string]: string;
}

@Injectable()
export class SchemaService {

  private schema: Observable<Schema> = this.http.get<Schema>(settings.getSchemaURL)
    .publishReplay(1) // this tells Rx to cache the latest emitted value
    .refCount(); // and this tells Rx to keep the Observable alive as long as there are any Subscribers
  // https://gist.github.com/Armenvardanyan95/eb8a078261eec2e4ff8c87bd8d7820a4#file-cached-response-ts

  public getCategoryTree(): Observable<CategoryTree> {
    return this.schema.map(res => res.categoryTree);
  }

  public getTicketTypes(): Observable<String[]> {
    return this.schema.map(res => res.typeList);
  }

  constructor(private http: HttpClient) {
  }


}
