TicketDesk
==========
TicketDesk is an issue tracking system for IT Help Desks.
TicketDesk is efficient and designed to do only one thing, facilitate communications between help desk staff and end users. The overriding design goal is to be as simple and frictionless for both users and help desk staff as is possible.

Blog
===========
You can read more about ongoing TicketDesk development [on my blog](http://www.reddnet.net/ticketdesk/).

Source Notes:
===========

The current shipping version is TicektDesk 2.1. TicketDesk 2.5 is in development but hasn't reached beta status yet.

- TicketDesk 2.1 requires Visual Studio 2012 or higher with .Net Framework 4.5 
- TicketDesk 2.5 requires Visual Studio 2012 or 2013 with .Net Framework 4.5 or higher.
  - Visual Studio 2013 with Update 3 recommended for Azure publishing

Current Development Information
===========

TicketDesk 2.5 is a full technology platfrom update. 

 - Asp.net MVC 5 with Razor Views
 - Entity Framework 6x with Code-First entity model
 - Aspnet.Identity for modern authentication and user management
 - Bootstrap / HTML5 / CSS3
 - OWIN/Katana Pipeline
 - Simple Injector (IoC / DI)

TicketDesk 2.5 will contain a small set of new features:

 - Watch / Follow Tickets
 - On-Screen first-run setup wizard
 - Advanced Admin Tools
   - Search Configuration
   - Database Management
   - Demo / Seed Data Management
   - Improved Email Settings and Diagnostics
 - Azure Deployability and Scaling
   - Email distribution managed by Azure WebJobs
   - Search powered by Azure Search Services
   - Attachments Stored in Azure Storage
   - Data hosted in Azure SQL


