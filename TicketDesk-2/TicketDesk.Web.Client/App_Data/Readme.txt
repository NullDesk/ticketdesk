The default configuration for TicketDesk 2.0 uses SQL 2008 R2 Express Edition.

The default database is a file-attached database that is stored in the app_data folder. This database includes both the TicketDesk tables as well as all objects required for the SQL membership, profile, and role providers (which are needed even in AD environments). This database can also be attached to a permanant instance of any full edition of SQL 2008 R2 (Standard, Enterprise, etc.) if you prefer. 

Unfortunatly, you cannot attach a database from SQL 2008 R2 to an older version of SQL server. Because of this, the distribution also includes a stock SQL 2005 database in the app_data/SQLExpress2005FileDatabase folder. This database can be used as a file-attached database in any version of SQL Server Express; including 2005, 2008, and 2008 R2. This 2005 database file can also be attached in full versions of SQL Server. 

You should remove any database files you aren't using before you deploy the application to a production environment. 

You can create your own database from scratch and create the TicketDesk objects from the scripts included in the app_data/NewDatabaseScripts folder. There are also scripts that can assist with upgrades from older versions of TicketDesk.For more information on upgrades and manually creating databases; please see refer to the full product documentation.