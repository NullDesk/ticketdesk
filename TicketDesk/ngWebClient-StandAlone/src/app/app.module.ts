import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule, Routes } from '@angular/router';

import { NotFoundComponent } from './not-found.component';
import { AppComponent } from './app.component';
import { FoobarComponent } from './foobar/foobar.component';

import { IndividualTicketViewComponent } from './individual-ticket-view/individual-ticket-view.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { TicketDetailEditorComponent } from './ticket-detail-editor/ticket-detail-editor.component';
import { TicketSubmitComponent } from './ticket-submit/ticket-submit.component';
import { TicketCenterComponent } from './ticket-center/ticket-center.component';

const appRoutes: Routes = [
	{ path: 'ticket/:ticketID', component: IndividualTicketViewComponent }, 
	{ path: 'submit', component: TicketSubmitComponent },
	{ path: 'center', component: TicketCenterComponent },
	{ path: '', component: FoobarComponent },
	{ path: '**', component: NotFoundComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    FoobarComponent,
	  TicketDetailEditorComponent,
	  TicketSubmitComponent,
    NotFoundComponent,
    IndividualTicketViewComponent,
    TicketCenterComponent,
  ],
  imports: [
    BrowserModule,
    NgbModule.forRoot(),
    AngularFontAwesomeModule,
    RouterModule.forRoot(appRoutes, {enableTracing: true})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
