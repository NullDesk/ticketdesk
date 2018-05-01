export interface TicketStub {
    ticketId: number;
    title: string;
    status: number;
    ownerId: string;
    assignedTo?: string;
    category: string;
    subcategory: string;
    priority?: string;
    createdDate: string;
    lastUpdateDate?: string;
  }

export const columnHeadings: {header: string, direction: string}[] = [
  {header: 'Ticket ID', direction: 'false'},
  {header: 'Title', direction: 'false'},
  {header: 'Status', direction: 'sortable'},
  {header: 'Priority', direction: 'false'},
  {header: 'Owner', direction: 'sortable'},
  {header: 'Assigned To', direction: 'false'},
  {header: 'Category', direction: 'false'},
  {header: 'Subcategory', direction: 'false'},
  {header: 'Category', direction: 'false'},
  {header: 'Created Date', direction: 'false'},
  {header: 'Last Update Date', direction: 'sortable'}
];

export const headingToBackendCol: Map<string, string> = new Map([
  ['Status', 'TicketStatus'],
  ['Owner', 'Owner'],
  ['Last Update Date', 'LastUpdateDate']
]);

export const ticketlistToUserDisplayMap: Map<string, string> = new Map([
  ['unassigned', 'Unassigned Tickets'],
  ['assignedToMe', 'My Assigned Tickets'],
  ['mytickets', 'My Tickets'],
  ['opentickets', 'Open Tickets'],
  ['historytickets', 'Closed Tickets'],
]);
