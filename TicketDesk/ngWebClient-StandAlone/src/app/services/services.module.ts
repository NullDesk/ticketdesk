import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileService } from './file.service';
import { SingleTicketService } from './single-ticket.service';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers: [FileService, SingleTicketService]
})
export class ServicesModule { }
