export interface TicketStub{
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

// Headings for table (displayed to User)
export const columnHeadings: string[] = ['TicketID', 'Title', 'Status', 'Priority', 'Owner', 'Assigned To', 'Category',
                                      'Created Date', 'Last Updated'];
