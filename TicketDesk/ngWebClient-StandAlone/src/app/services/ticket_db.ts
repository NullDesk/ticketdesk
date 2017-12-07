import { Ticket, Logs } from '../models/data'

export var tickets:[Ticket] = [

	{"ticketId": 1111, 
	"projectId": 10, 
	"priority": 5,
    "comment": "Initial Submission", 
    "details": "Two weeks ago, my EVGA GTX 780 Ti of 3.5 years started to malfunction (artifacts, display disruptions, etc.) and shortly after that, finally died. This was especially annoying because it was only half a year after EVGA's warranty expired. So I researched online and bought an EVGA GTX 1080 Ti (which I can still return). This card is overkill for my setup, I do know that, especially for gaming with 1920x1080. However, I intend to buy a second monitor with either a 2k or 4k resolution which should be enough to keep up with this beast.", "title": "My Computer is broken.", 
    "ticketType": "type test", 
    "category": "Hardware", 
    "subcategory": "Graphics Card", 
	"owner" : "1000",
	"assignedTo" : "1000",
	'createdDate': "11/12/17",
	"status" : "closed", 
	"tagList": "tag1 tag2"},

    {"ticketId": 2222, 
    "projectId": 11,
    "comment": "test comment 1",
    "title": "test title 1",
    "details": "detail test 1",
    "ticketType": "type test 1",
    "category": "test category 1",
    "subcategory": "subcategory test 1",
	"owner": "1001",
	"assignedTo" : "1002",
	"status" : "open",
	"priority": 1,
	'createdDate': "11/13/17",
	"tagList": "List test 1" },
	
	{"ticketId": 3333, 
	"projectId": 12,
	"priority": 1,
    "comment": "test comment 2",
    "title": "test title 2",
    "details": "detail test 2",
    "ticketType": "type test 1",
    "category": "test category 1",
    "subcategory": "subcategory test 2",
	"owner": "1001",
	"assignedTo" : "1002",
	"status" : "open",
	'createdDate': "11/13/17",
	"tagList": "List test 2"
	},

	{"ticketId": 4444, 
	"projectId": 13,
	"priority": 2,
    "comment": "test comment 3",
    "title": "test title 3",
    "details": "detail test 3",
    "ticketType": "type test 1",
    "category": "test category 3",
    "subcategory": "subcategory test 3",
	"owner": "1003",
	"assignedTo" : "1001",
	"status" : "closed",
	'createdDate': "11/11/17",
	"tagList": "List test 3" },

	{"ticketId": 5555, 
	"projectId": 14,
	"priority": 2,
    "comment": "test comment 4",
    "title": "test title 4",
    "details": "detail test 4",
	"ticketType": "type test 1",
	'createdDate': "11/10/17",
    "category": "test category 3",
    "subcategory": "subcategory test 4",
	"owner": "1001",
	"assignedTo" : "1002",
	"status" : "open",
	"tagList": "List test 4" },

	{"ticketId": 6666, 
	"projectId": 15,
	"priority": 4,
    "comment": "test comment 5",
    "title": "test title 5",
    "details": "detail test 5",
    "ticketType": "type test 1",
    "category": "test category 5",
    "subcategory": "subcategory test 5",
	"owner": "1002",
	'createdDate': "11/16/17",
	"status" : "closed",
	"tagList": "List test 5" }

];

export var logs:[Logs] = [
	{
		"ticketId": 1111,
		"entries": [
			{
				"owner": "1000",
				"description": "To be, or not to be: that is the question: Whether ‘tis nobler in the mind to suffer The slings and arrows of outrageous fortune, Or to take arms against a sea of troubles, And by opposing end them? To die: to sleep; No more; and by a sleep to say we end",
				"date": "Tuesday, November 14, 2017 9:00 PM",
				"status_change": "closed ticket"
			},

			{
				"owner": "1002",
				"description": "I am looking into why the graphics card is not working. Maybe not it's plugged in?",
				"date": "Tuesday, November 14, 2017 8:45 PM",
				"status_change": "Started work on ticket"
			},

			{
				"owner": "1000",
				"description": "I am assigning tech #123 because he is the most knowlege about graphics cards",
				"date": "Tuesday, November 14, 2017 8:30 PM",
				"status_change": "assigned to tech"
			},
			{
				"owner": "1001",
				"description": "Two weeks ago, my EVGA GTX 780 Ti of 3.5 years started to malfunction (artifacts, display disruptions, etc.) and shortly after that, finally died.",
				"date": "Tuesday, November 14, 2017 8:13 PM",
				"status_change": "user created the ticket"
			}

		]
	},

	{
		"ticketId": 2222,
		"entries": [
			{
				"owner": "1001",
				"description": "Test ticket Submission",
				"date": "Tuesday, November 14, 2017 9:00 PM",
				"status_change": "Created Ticket"
			}

		]
	},

	{
		"ticketId": 3333,
		"entries": [
			{
				"owner": "1001",
				"description": "Test ticket Submission",
				"date": "Tuesday, November 14, 2017 9:00 PM",
				"status_change": "Created Ticket"
			}
		]
	}

];
