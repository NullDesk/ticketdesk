import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileService } from './file.service';
import { SingleTicketService } from './single-ticket.service';
import { SubmitTicketService } from './submit-ticket.service';
import { UploadService } from './upload.service';
import { HttpModule } from '@angular/http';
import { MultiTicketService } from './multi-ticket.service';
import { SchemaService } from './schema.service';
import { AdContactService } from './ad-contact.service';

@NgModule({
  imports: [
    CommonModule,
    HttpModule
  ],
  declarations: [],
  providers: [
    FileService,
    SingleTicketService,
    SubmitTicketService,
    UploadService,
    MultiTicketService,
    SchemaService,
    AdContactService
  ]
})
export class ServicesModule { }
