import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';


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
import { ActivityLogComponent } from './activity-log/activity-log.component';
import { ContactInfoComponent } from './contact-info/contact-info.component';
import { SearchBoxComponent } from './search-box/search-box.component';

import { SingleTicketService } from './services/single-ticket.service';


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
	ActivityLogComponent,
	ContactInfoComponent,
	SearchBoxComponent,
  ],
  imports: [
    BrowserModule,
    NgbModule.forRoot(),
    AngularFontAwesomeModule,
	  ReactiveFormsModule,
    RouterModule.forRoot(appRoutes, {enableTracing: true})
  ],
  providers: [SingleTicketService],
  bootstrap: [AppComponent]
})
export class AppModule { }
