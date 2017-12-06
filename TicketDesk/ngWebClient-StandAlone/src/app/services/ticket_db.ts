import { Ticket, Logs } from '../models/data'

export let tickets:[Ticket] = [

    {"ticketId": 123456, "projectId": 1, 
    "comment": "Initial Submission", 
    "details": "Two weeks ago, my EVGA GTX 780 Ti of 3.5 years started to malfunction (artifacts, display disruptions, etc.) and shortly after that, finally died. This was especially annoying because it was only half a year after EVGA's warranty expired. So I researched online and bought an EVGA GTX 1080 Ti (which I can still return). This card is overkill for my setup, I do know that, especially for gaming with 1920x1080. However, I intend to buy a second monitor with either a 2k or 4k resolution which should be enough to keep up with this beast.", "title": "My Computer is broken.", 
    "ticketType": "type test", 
    "category": "Hardware", 
    "subcategory": "Graphics Card", 
    "owner" : "122902", 
    "tagList": ["tag1", "tag2"]
}/*,

    {"ticketId": 10, 
    "projectId": 11,
     "comment": "test comment 1",
      "title": "test title 1",
       "details": "detail test 1",
        "ticketType": "type test 1",
         "category": "test category 1",
          "subcategory": "subcategory test 1",
           "owner": "owner test 1",
            "tagList": ["tagList test 1"] }
*/
];

let logs:[Logs] = [
	{"ticketId": 123456,
	"entries" : [
	{	"owner" : "Tech #123",
		"description" : "To be, or not to be: that is the question: Whether ‘tis nobler in the mind to suffer The slings and arrows of outrageous fortune, Or to take arms against a sea of troubles, And by opposing end them? To die: to sleep; No more; and by a sleep to say we end",
		"date" : "Tuesday, November 14, 2017 9:00 PM",
		"status_change" : "closed ticket"},
	
	{	"owner" : "Tech #123",
		"description" : "I am looking into why the graphics card is not working. Maybe not it's plugged in?",
		"date" : "Tuesday, November 14, 2017 8:45 PM",
		"status_change" : "Started work on ticket"},
	
	{	"owner" : "Admin",
		"description" : "I am assigning tech #123 because he is the most knowlege about graphics cards",
		"date" : "Tuesday, November 14, 2017 8:30 PM",
		"status_change" : "assigned to tech"},
	{	"owner" : "Regular",
		"description" : "Two weeks ago, my EVGA GTX 780 Ti of 3.5 years started to malfunction (artifacts, display disruptions, etc.) and shortly after that, finally died.",
		"date" : "Tuesday, November 14, 2017 8:13 PM",
		"status_change" : "user created the ticket"}	
	
	]
	},
	
	{"ticketId": 10,
	"entries" : [
	{	"owner" : "User",
		"description" : "Test ticket Submission",
		"date" : "Tuesday, November 14, 2017 9:00 PM",
		"status_change" : "Created Ticket"}	
	
	]
	}

];

/*export let user:{} = {
	"user_id" : "122902"
	"first" : "Daniel",
	"last" : "Maida",
	"phone" : "360-823-8585",
	"email" : "daielmaida2@gmail.com"

};*/
