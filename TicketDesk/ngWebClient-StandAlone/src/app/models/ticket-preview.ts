export interface TicketPreview {
    ticketId: number;
	title: string;
	status: string; 
	priority?: string; 
	owner: string; 
	assigned?: string;
	category: string; 
    subcategory: string;		
	createdDate: number; //Change this?
}