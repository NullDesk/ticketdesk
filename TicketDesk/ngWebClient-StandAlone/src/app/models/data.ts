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
	owner: string, 
	tagList: [string]	
}

export const BLANK_TICKET: Ticket = {ticketId: -1,
projectId: -1,
	comment: '', title: '', details: '', ticketType: '',
	category: '', subcategory: '', owner: '', tagList: ['']

} 

export interface Logs {
	ticketId : number,
	entries : [Entry]
}

export interface Entry{
	owner: string,
	description : string,
	date : string,
	status_change : string
}

