import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileService } from './file.service';
import { SingleTicketService } from './single-ticket.service';
import { SubmitTicketService } from './submit-ticket.service';
import { UploadService } from './upload.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MultiTicketService } from './multi-ticket.service';
import { SchemaService } from './schema.service';
import { UserService } from './user.service';
import { WindowsAuthenticationInterceptorService } from './windows-authentication-interceptor.service';
import { SearchService } from './search.service';
import { RedirectService } from './redirect.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  declarations: [],
  providers: [
    FileService,
    SingleTicketService,
    SubmitTicketService,
    UploadService,
    MultiTicketService,
    SearchService,
    SchemaService,
    RedirectService,
    UserService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: WindowsAuthenticationInterceptorService,
      multi: true,
    }
  ]
})
export class ServicesModule { }
