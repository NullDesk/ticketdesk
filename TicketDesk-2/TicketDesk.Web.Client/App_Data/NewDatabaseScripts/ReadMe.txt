
These scripts will NOT create the database(s) for you. You will need to create the database first.

These scripts will populate an empty database with the tables and other objects necessary for TicketDesk:

Run them in this order:

1_TicketDeskObjects.sql

	This script contains the ticket desk tables and related objects, plus the default values needed for the settings table.

2_SecurityObjects.sql

	If you prefer you can use the aspnet_regsql.exe tool that ships with asp.net instead of using this script. This script will create the security objects for the asp.net membership, roles, and profile providers. You can run this against the main TicketDesk database, or you can create a second database for use with the security providers. 

3_SecurityDefaultUsers.sql

	This script will create default user accounts for three users. You do not have to run this script; instead you can use the aspnet configuration utility to create an initial admin users, then from there use the online tools to create and manage other user accounts. 

	Here are the users created:
		
		user = admin
			pwd = admin
			This is an administrative user in all three TD roles

		user = toastman
			pwd = toastman
			This is an example end-user, only in the submitters TD role.

		user = otherstaffer
			pwd = otherstaffer
			This is an example help-desk user, in the help desk and submitter roles; but not an administrator