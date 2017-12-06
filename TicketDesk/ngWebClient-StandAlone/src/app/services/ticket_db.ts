import { Ticket } from '../models/data'

export let tickets:[Ticket] = [

    {"ticketId": 123456, "projectId": 1, 
    "comment": "Initial Submission", 
    "details": "Two weeks ago, my EVGA GTX 780 Ti of 3.5 years started to malfunction (artifacts, display disruptions, etc.) and shortly after that, finally died. This was especially annoying because it was only half a year after EVGA's warranty expired. So I researched online and bought an EVGA GTX 1080 Ti (which I can still return). This card is overkill for my setup, I do know that, especially for gaming with 1920x1080. However, I intend to buy a second monitor with either a 2k or 4k resolution which should be enough to keep up with this beast.", "title": "My Computer is broken.", 
    "ticketType": "type test", 
    "category": "Hardware", 
    "subcategory": "Graphics Card", 
    "owner" : "122902", 
    "tagList": ["tag1", "tag2"]
},

    {"ticketId": 10, "projectId": 11, "comment": "test comment 1", "title": "test title 1", "details": "detail test 1", "ticketType": "type test 1", "category": "test category 1", "subcategory": "subcategory test 1", "owner": "owner test 1", "tagList": "tagList test 1" }

];

export let user:{} = {
	"user_id" : "122902"
	"first" : "Daniel",
	"last" : "Maida",
	"phone" : "360-823-8585",
	"email" : "daielmaida2@gmail.com"

};
