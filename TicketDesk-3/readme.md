TicketDesk
==========
TicketDesk is an issue tracking system for IT Help Desks.
TicketDesk is efficient and designed to do only one thing, facilitate communications between help desk staff and end users. The overriding design goal is to be as simple and frictionless for both users and help desk staff as is possible.

Source Notes:
===========
- TicketDesk 3 requires Visual Studio 2013 with .Net Framework 4.5.1 or higher.
- The build process uses durandal's weyland build tool to optimize javascript files. 
  - You must install node.js and the weyland node package.
  - The Visual Stuidio project automatically runs weyland on post-build.
  - TicketDesk itself is *not* a node.js project
  - More information can be found on [durandal's site here](http://durandaljs.com/documentation/Building-with-Weyland/)

Information
===========

TicketDesk 3 will contain a small set of new features:

 - Multiple Ticket Owners
 - Multiple Ticket AssignedTo
 - Ticket Notification Subscribers
 - Tag Management
 - Mobile Friendly
 - Localization
 - Real-time updates and collaboration


TicketDesk 3.x is a re-write of the TicketDesk 2 system on the following technologies:

 - ASP.NET MVC 5
 - ASP.NET WebAPI 2
 - Knockout.js
 - Durandal
 - Breeze.js
 - SignalR
 - Bootstrap
 - i18next
 - Aspnet.Identity
 - EntityFramework Code-First
 - Windows Azure (optional)
  
