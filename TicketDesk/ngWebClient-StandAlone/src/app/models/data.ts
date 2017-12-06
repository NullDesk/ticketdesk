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

