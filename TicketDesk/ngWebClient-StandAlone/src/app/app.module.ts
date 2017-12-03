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
import { ReportViewComponent } from './report-view/report-view.component';
import { TicketCenterComponent } from './ticket-center/ticket-center.component';
import { SettingsViewComponent } from './settings-view/settings-view.component';
import { TicketListRowComponent } from './ticket-list-row/ticket-list-row.component';
import { TicketListComponent } from './ticket-list/ticket-list.component';
import { FormsModule } from '@angular/forms';

const appRoutes: Routes = [
	{ path: 'ticket/:ticketID', component: IndividualTicketViewComponent }, 
	{ path: 'submit', component: TicketSubmitComponent },
	{ path: 'center', component: TicketCenterComponent },
	{ path: 'report', component: ReportViewComponent },
	{ path: 'settings', component: SettingsViewComponent },
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
	ReportViewComponent,
	SettingsViewComponent,
	TicketListRowComponent,
	TicketListComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    NgbModule.forRoot(),
    AngularFontAwesomeModule,
    RouterModule.forRoot(appRoutes, {enableTracing: true})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
