import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ServicesModule } from './services/services.module';

import { NotFoundComponent } from './not-found.component';
import { AppComponent } from './app.component';
import { AttachFileComponent } from './attach-file/attach-file.component';
import { SingleTicketViewComponent } from './single-ticket-view/single-ticket-view.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { TicketDetailEditorComponent } from './ticket-detail-editor/ticket-detail-editor.component';
import { TicketSubmitComponent } from './ticket-submit/ticket-submit.component';
import { ReportViewComponent } from './report-view/report-view.component';
import { TicketCenterComponent } from './ticket-center/ticket-center.component';
import { SettingsViewComponent } from './settings-view/settings-view.component';
import { TicketListComponent } from './ticket-list/ticket-list.component';
import { ActivityLogComponent } from './activity-log/activity-log.component';
import { ContactInfoComponent } from './contact-info/contact-info.component';
import { SearchBoxComponent } from './search-box/search-box.component';
import { SingleTicketService } from './services/single-ticket.service';
import { AdUserComponent } from './ad-user/ad-user.component';
import { TicketCenterListComponent } from './ticket-center-list/ticket-center-list.component';

const appRoutes: Routes = [
	{ path: 'ticket/:ticketID', component: SingleTicketViewComponent },
	{ path: 'submit', component: TicketSubmitComponent },
	{ path: 'center', component: TicketCenterComponent },
	{ path: 'report', component: ReportViewComponent },
	{ path: 'settings', component: SettingsViewComponent },
	{ path: '', pathMatch: 'full', redirectTo: 'center'},
	{ path: '**', component: NotFoundComponent }
];

@NgModule({
		declarations: [
		AppComponent,
		TicketDetailEditorComponent,
		TicketSubmitComponent,
		NotFoundComponent,
		SingleTicketViewComponent,
		TicketCenterComponent,
		ReportViewComponent,
		SettingsViewComponent,
		TicketListComponent,
		ActivityLogComponent,
		ContactInfoComponent,
		SearchBoxComponent,
		AttachFileComponent,
		AdUserComponent,
		TicketCenterListComponent,
  ],
  imports: [
	  ServicesModule,
    BrowserModule,
    NgbModule.forRoot(),
    AngularFontAwesomeModule,
	  ReactiveFormsModule,
	  FormsModule,
	RouterModule.forRoot(appRoutes, {enableTracing: true})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
