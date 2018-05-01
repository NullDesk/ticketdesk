import { Injectable, Component, OnInit } from '@angular/core';
import { TicketStub, ticketlistToUserDisplayMap } from '../models/ticket-stub';
import { userPermissions } from '../models/user-details';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MultiTicketService } from '../services/multi-ticket.service';
import { UserService } from '../services/user.service';
import { NgbTabChangeEvent } from '@ng-bootstrap/ng-bootstrap/tabset/tabset';

@Component({
  selector: 'app-ticket-center',
  templateUrl: './ticket-center.component.html',
  styleUrls: ['./ticket-center.component.css']
})


export class TicketCenterComponent implements OnInit {
  tabNames: string[] = ['mytickets', 'opentickets', 'historytickets'];
  ticketList: TicketStub[];
  pagination: {current: number, max: number } = {current: 1, max: 2};
  currentList;
  listReady: Boolean = false;
  tabsReady: Boolean = false;
  resetListSettings: Boolean = true;

  constructor(private multiTicketService: MultiTicketService,
              private userService: UserService) {
  }

  ngOnInit() {
    this.userService.getUserPermissions()
        .subscribe(permissions => {
          if (permissions === userPermissions.admin || permissions === userPermissions.resolver) {
            this.tabNames.unshift('unassigned', 'assignedToMe');
          }
          this.currentList = this.tabNames[0];
          this.tabsReady = true;
          // Sending empty string, gets the default page from the backend, dependent on their permissions.
          // mytickets for standard users, unassigned for resolvers and admins
          this.getTicketList(this.currentList, 1);
        });
  }

  getTicketList(listName: string, page: number): void {
    console.log('Getting ticketlist for', listName, 'at page ', page);
    if (this.resetListSettings) {
      this.multiTicketService.resetFilterAndSort()
          .subscribe( res => {
            this.setNewList(listName, page);
            this.resetListSettings = false;
          });
    } else {
      this.setNewList(listName, page);
    }
  }

  setNewList(listName: string, page: number) {
    this.multiTicketService.getTicketList(listName, page)
        .subscribe(ticketList => {
          this.ticketList = ticketList;
          this.pagination.max = (ticketList.length === 0) ? page : page + 1;
          this.pagination.current = page;
          this.listReady = true; });
  }

  onTabChange(event: NgbTabChangeEvent) {
    // This makes the ticket list compoent reload
    this.listReady = false;
    this.resetListSettings = true;
    this.currentList = event.nextId;
    this.getTicketList(event.nextId, 1);
  }

  pageChange(page: number) {
    this.getTicketList(this.currentList, page);
  }

  sortTrigger(colName: string) {
    console.log('Getting sorting for', colName);
    this.multiTicketService.sortList(this.pagination.current, this.currentList, colName, false)
        .subscribe(ticketList => {
          console.log('Got Sorted TicketList:', JSON.stringify(ticketList));
          this.ticketList = ticketList;
        });
  }

  convertListNameToDisplayStr(str: string) {
    return ticketlistToUserDisplayMap.has(str) ? ticketlistToUserDisplayMap.get(str) : str;
  }
}
