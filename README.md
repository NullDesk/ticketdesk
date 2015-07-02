
<a href="https://zenhub.io"><img src="https://raw.githubusercontent.com/ZenHubIO/support/master/zenhub-badge.png"></a>


TicketDesk
==========
TicketDesk is an issue tracking system for IT Help Desks.

TicketDesk is efficient and designed to do only one thing, facilitate communications between help desk staff and end users. The overriding design goal is to be as simple and frictionless for both users and help desk staff as is possible.

<img src="https://raw.githubusercontent.com/NullDesk/TicketDesk/develop/td25.png" title="TicketDesk 2.5"  />

Documenation
===========
Documentation can be found in the [TicketDesk GitHub Wiki](https://github.com/StephenRedd/TicketDesk/wiki)

Project Status:
===========

[TicketDesk 2.5 is in beta](https://github.com/StephenRedd/TicketDesk/releases/tag/td2-v2.5.0). We are on track for an August 1<sup>st</sup> stable release. 

View a working [demo of TicketDesk 2.5 here](http://ticketdesk2.azurewebsites.net/). The demo is automatically deployed by continuous integration from the development branch.

The current stable version is [TicektDesk 2.1](https://github.com/StephenRedd/TicketDesk/releases/tag/td2-v2.1.3). 

- TicketDesk 2.5 requires Visual Studio 2013 with .Net Framework 4.5.2 or higher.
  - Visual Studio 2013 with Update 4 or higher is required for Azure publishing
  
- TicketDesk 2.1 requires Visual Studio 2012 or higher with .Net Framework 4.5

Getting started:
===========

Development
-----------

Getting started with TicketDesk development should be a "clone, open, and run" experience. You need Visual Studio 2013 with Update 4 or higher, SQL 2012 LocalDB, and Git.   

- Clone the github repository
- Switch to the desired branch (development has the latest version, master has latest release ready version)  
- Open the project in Visual Studio 2013 or higher
- Hit F5 to run/debug the application
- Code, Contribute, and Enjoy!

Install on Windows Server & IIS
-----------

These are basic instructions for installing a [pre-compiled distribution](https://github.com/NullDesk/TicketDesk/releases) of TicketDesk 2.5 to a single Windows Server with IIS 8 or higher. 
For more detailed instructions, and information about other deployments scenarios, including Azure deployment, please see the [GitHub wiki](https://github.com/NullDesk/TicketDesk/wiki).

- Server Requirments:
  - Make sure IIS is installed with the options necessary sub-options needed to run Asp.Net MVC 5 applications
  - Make sure .Net Framework 4.5.2 is installed on the target server
  - Make sure you have access to SQL server 2008 or higher
    - The stock web.config is pre-set to use a locally installed instance of SQL Server 2012 LocalDB
    - For more information about SQL Server configuration and connection strings please refer to the [GitHub wiki](https://github.com/NullDesk/TicketDesk/wiki)
- Create an IIS Site or right-click an existing site and select "Add Application" to create a new Virtual Directory
  - Select or Create an application pool that uses an integrated pipeline
  - Make sure the application pool is set for .Net Framework 4.0
- Download the desired precompiled distribution from [GitHub releases](https://github.com/NullDesk/TicketDesk/releases) 
- Copy the contents of the zip file into the IIS site's physical folder
  - Make sure the application pool user has read access to the web site root folder and all sub-folders (if using the default applicaiton pool identity, just grant the '[machine]\users' group the necessary permissions)
  - Make sure the application pool user has write access to the /app_data folder 
- Open a web browser, and browse to the web site's root URL
- The first-run-setup page should appear
  - Review the settings shown; make everything is accurate before continuing
  - Click the Create database button

