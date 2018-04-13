export interface Ticket {
  ticketId: number;
  projectId: number;
  title: string;
  details: string;
  priority?: string;
  ticketType: string;
  category: string;
  subcategory: string;
  ownerId: string; // userId of person who is having the issue
  assignedTo: string;
  status: number;
  tagList: string;
  createdDate?: string;
}

export function getTicketStatusText(enumInt: number) {
  return (enumInt > -1 && enumInt < 4) ? ['Active', 'Needs More Information', 'Resolved', 'Closed'][enumInt] : '';
}

export const BLANK_TICKET: Ticket = {
  ticketId: -1,
  projectId: -1,
  title: '',
  details: '',
  ticketType: '',
  category: '',
  subcategory: '',
  ownerId: '',
  assignedTo: '',
  status: 0,
  tagList: ''
};




