TicketDesk
==========
TicketDesk is an issue tracking system for IT Help Desks.

TicketDesk is efficient and designed to do only one thing, facilitate communications between help desk staff and end users. The overriding design goal is to be as simple and frictionless for both users and help desk staff as is possible.

<img src="http://download.codeplex.com/download?ProjectName=TicketDesk&DownloadId=193983" title="TicketDesk 2"  />

Documenation
===========
Documentation can be found in the [TicketDesk GitHub Wiki](https://github.com/StephenRedd/TicketDesk/wiki)

You can read more about ongoing TicketDesk development [on my site](http://www.reddnet.net/ticketdesk/).

Source Notes:
===========

The current shipping version is [TicektDesk 2.1]("https://github.com/StephenRedd/TicketDesk/releases/tag/td2-v2.1.2"). 

TicketDesk 2.5 is in development and should be ready for beta releases in January 2015.

- TicketDesk 2.1 requires Visual Studio 2012 or higher with .Net Framework 4.5 
- TicketDesk 2.5 requires Visual Studio 2012 or 2013 with .Net Framework 4.5 or higher.
  - Visual Studio 2013 with Update 3 recommended for Azure publishing

Current Development Information
===========

A [demo of TicketDesk 2.5]("http://ticketdesk2.azurewebsites.net/") is available. It is just an alpha, so expect rough-edges

TicketDesk 2.5 is a full technology platfrom update. 

 - Asp.net MVC 5 with Razor Views
 - Entity Framework 6x Code-First
 - Aspnet.Identity
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


