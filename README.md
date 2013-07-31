TicketDesk
==========

TicketDesk is an issue tracking system for IT Help Desks. 

TicketDesk is efficient and designed to do only one thing, facilitate communications between help desk staff and end users. The overriding design goal is to be as simple and frictionless for both users and help desk staff as is possible.

Source Notes:
===========

- The TicketDesk 3 project requires Visual Studio 2012 and .Net Framework 4.5
- The build process runs durandal's optimizer.exe; TicketDesk 3 does not use node.js, but you must install node.js for the post-build optimizer to run. 

Information
=========== 

This is the github repository, initially created to host development of TicketDesk version 3.x. 

TicketDesk 3 will contain a small set of new features:

- Multiple Ticket Owners
- Multiple Ticket AssignedTo
- Ticket Notification Subscribers
- Tag Management
- Mobile Views
- Localization
- Real-time updates and collaboration

TicketDesk 3.x is a re-write of the TicketDesk system, and uses the following technologies:

- HTML 5 HotTowel SPA UI
	- ASP.NET MVC 
	- WebAPI
	- Knockout.js
	- Durandal
	- Breeze.js
	- SignalR
	- Bootstrap
	
- Windows Azure Compatibility
- oAuth and local security
	- Using ADFS 2.0 via oAuth for ActiveDirectory environments
- EntityFramework Code-First 
	- SQL versions including:
		- SQL CE 4
		- SQL Server LocalDB
		- SQL Server
		- SQL Azure
	- Database Migrations
