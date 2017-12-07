import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileService } from './file.service';
import { SingleTicketService } from './single-ticket.service';
import { MultiTicketService } from './multi-ticket.service';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers: [FileService, SingleTicketService, MultiTicketService]
})
export class ServicesModule { }
