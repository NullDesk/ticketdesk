import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule, Routes } from '@angular/router';

import { NotFoundComponent } from './not-found.component';
import { AppComponent } from './app.component';
import { FoobarComponent } from './foobar/foobar.component';

const appRoutes: Routes = [
	{ path: '', component: FoobarComponent },
	{ path: '**', component: NotFoundComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    FoobarComponent,
	  NotFoundComponent,
  ],
  imports: [
    BrowserModule,
    NgbModule.forRoot(),
    RouterModule.forRoot(appRoutes, {enableTracing: true})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
