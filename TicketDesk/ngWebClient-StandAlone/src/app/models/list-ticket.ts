export interface ListTicket {
    ticketId: number;
    title: string;
    status: number;
    owner: string;
    assignedTo?: string;
    category: string;
    subcategory: string;
    priority?: string;
    createdDate: string;
    updatedDate?: string;
  }

// Columns to displayed in a ticket list. Does not include ticketId, or title, as that will always be displayed.
export const displayCols: string [] = ['status', 'priority', 'owner', 'assignedTo', 'category', 'subcategory',
                                       'createdDate', 'updatedDate'];
// Headings for table (displayed to User)
export const colHeadings: string[] = ['TicketID', 'Title', 'Status', 'Priority', 'Owner', 'Assigned To', 'Category',
                                      'Created Date', 'Last Updated'];
