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
