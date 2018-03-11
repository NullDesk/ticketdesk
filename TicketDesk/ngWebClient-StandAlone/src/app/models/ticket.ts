export interface Ticket {
	ticketId: number,
	projectId: number, 
	comment: string, 
	title: string, 
	details: string, 
	priority?: string, 
	ticketType: string, 
	category: string, 
	subcategory: string,	
	ownerId: string, // userId of person who is having the issue
	assignedTo: string,
	status: string, 
	tagList: string,
	createdDate?: string	
}

export const BLANK_TICKET: Ticket = {
	ticketId: -1,
	projectId: -1,
	comment: '', 
	title: '', 
	details: '', 
	ticketType: '',
	category: '', 
	subcategory: '', 
	ownerId: '',
	assignedTo: '', 
	status: '', 
	tagList: ''

} 




