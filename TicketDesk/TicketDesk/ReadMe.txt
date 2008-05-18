Upgrade:

	To upgrade TicketDesk 1.0 to 1.1:
	
	     * Copy the updated web application files to the web server.
	     
         * Run the scrip "/app_data/Update_1.0_to_1.1.SQL" against your database
         
		 * Edit the web.config as appropriate (see the section Configuration section for more info)
		 
		      - Please note, the web.config contains a new section that you may need to edit:
		      
		          <location path="Admin">
					<system.web>
					  <authorization>
						<allow roles="Administrators"/>
						<deny users="*"/>
					  </authorization>
					</system.web>
				  </location>
				  
				  
				You will need to put the correct role name for your SQL role or AD group to allow admin users access to the new admin tools. 

Installation:

    Pre-compiled distributions: 
    	
        * Edit the web.config file with settings appropriate to your environment (see configuration). The default settings are valid for most IIS 6 or IIS 7 web servers and use file attached databases contained within the distribution's app_data folder via SQL Express 2005.
        	  
        * If you wish to use a full version of SQL Server see Database Installation section for details on how to create the database(s).
        	  
        * Create an IIS application. This can be a new web site, a folder within an existing web site, or a virtual directory. If using IIS 7 you can setup your application to use either the integrated pipeline (recommended) or the classic pipeline. 
        	  
        * Copy the files from the distribution to your web server.

    Source Distribution:

        * Open the solution in Visual Studio 2008 or Visual Web Developer 2008. 
        	  
        * Edit the web.config file with settings appropriate to your environment (see configuration). The default settings are valid for most IIS 6 or IIS 7 web servers and use file attached databases contained within the distribution's app_data folder via SQL Express 2005.
        	  
        * If you wish to use a full version of SQL Server see Database Installation section for details on how to create the database(s).
        	 
        * The source distribution can run directly in the built-in web server that ships with VS 2008 or VWD 2008. 
        	  
        * The source code is a Web Application Project. You will have to "build" the solution before running the application (unlike standard web site projects where you can just edit and run).
        	  
        * You can deploy from VS 2008 or VWD 2008 to a web server by using the "Publish Web" feature or by manually copying the files to the web server. Don't forget to build the application in "Release" mode before you deploy it into production.
     

Configuration:

    TicketDesk configuration is largely performed by editing the web.config file. 

    Web.config:

        The default web.config file is setup with default values will work on IIS 6 or IIS 7 using the SQL Express 2005 database files included in app_data.

    WebSql.config:
    	
        This is a copy of the default web.config. Replace the web.config with a renamed copy of this file to reset back to the default settings.
    	
    WebAd.config:

        This is a copy of web.config modified for use with Active Directory. Unlike the SQL configuration file, this file will require manual changes to run in any environment. Replace the web.config file with a copy of this file if you plan to use AD integration.
    	
    Common Web.Config settings:

        No matter if using SQL or AD versions of the web.config, there are some settings that apply to both:
    	
            * <appsettings> section:
    		
                ** AllowSubmitterRoleToEditPriority: true or false - if true all users will be able to set or edit the priority of tickets. If false, only help desk staff or admins can set or edit the priority.
                		
                ** AllowSubmitterRoleToEditTags: true or false - similar to priority settings; if true all users can edit tags for tickets. If false only help desk staff or anmins can set or edit tags.
                		  
                ** EnableEmailNotifications: true or false - if true TicketDesk will send email notifications to the owner and/or assigned user when changes are made to a ticket. 
                		  
                ** FromEmailAddress: the email address that will show up in the "from" field for notification emails.
                	      
                ** FromEmailDisplayName: the friendly name that will show up in the from field for notification emails. 
    	      
            * <connectionStrings> By default TicketDesk uses two SQL Express databases in the app_data folder (as file-attached databases).  One database contains the tickets and related information. The other is for the SQL Security providers (membership, roles, profiles). Even when using AD security, there will still be a SQL security database required for the profile fields. If you setup your installation to use different databases you can edit the connection strings as appropriate. 
    	
            * <system.net><mailSettings> If using Email notifications you must configure this section with valid settings for your the mail server that will relay email for the application. 
            		
            * <machineKey> If you wish to store passwords in an encrypted format you should specify a valid machine key. This section is commented out in the default configuration since the membership provider is setup to use clear text passwords.
    	
    Configuration Settings with SQL Server Security:

        SQL Server based configurations: The default web.config file in TicketDesk is setup to use SQL server security. The WebSql.config file is simply a copy of the default web.config file to use for reference.
    	
        The default configuration should work as long as you have SQL Server Express installed on the web server and the web server has a locally installed SMTP server on port 25.
    	
        There are several common settings that you may want to review and change to configure TicketDesk for your Environment. 
    	
            * <appsettings> section:
    	
                ** TicketSubmittersRoleName: The name of the user role for users that are allowed to submit and view tickets. These users cannot be assigned to work on tickets but can comment on open tickets. 
    		  
                ** HelpDeskStaffRoleName: The name of the user role for users that will be allowed to work on tickets. These users can do everything in TicketDesk except manage user accounts or change TicketDesk configuration settings. 
                		  
                ** AdministrativeRoleName: The name of the user role for users that can edit configuration settings and manage users in TicketDesk. 
    		
            * <membership> If you wish, you can change settings for the membership provider here. A common change would be to switch to using encrypted passwords (which requires a machine key too).
            		
            * <authorization> This specifies which roles are allowed to access TicketDesk. If you customize the role names in the <appsettings> section you will also have to change the roles in <authorization> as well.
    		
    		* <location path="Admin"><system.web><authorization> specifies which role is allows to access the admin pages. 
    		
    Configuration Settings with Active Directory Security:

        The WebAd.config file contains the settings that are needed when using TicketDesk with Active Directory security. 
    	
        To use AD, you will need to replace web.config with a copy of WebAd.config then edit the settings as appropriate for your environment.
    	
            * <appsettings>
            	
                ** ActiveDirectoryDomain: The name of your AD domain. Usually looks like a DNS domain name (SomeDomain.com)
                		  
                ** ActiveDirectoryUser: The name of an AD user account that has at least read access to AD. 

                ** ActiveDirectoryUserPassword: The password for the AD user account

                ** TicketSubmittersRoleName: The name of the AD group that contains the users that are allowed to submit and view tickets. These users cannot be assigned to work on tickets but can comment on open tickets.  
                		
                ** HelpDeskStaffRoleName: The name of the AD group that contains the users that will be allowed to work on tickets. These users can do everything in TicketDesk except manage user accounts or change TicketDesk configuration settings. 
                		  
                ** AdministrativeRoleName: The name of the AD group that contains the users that can edit configuration settings and manage users in TicketDesk. 

            * <authorization> This specifies which AD groups are allowed to access TicketDesk. These should be the same group names as are specified in the <appsettings> section.
    		
    		* <location path="Admin"><system.web><authorization> specifies which AD group is allowed to access the admin pages. 
	
Security 

    TicketDesk can be configured for use with SQL Server providers or Active Directory (SQL is the default). 

    SQL Server Security:

        In this configuration, TicketDesk maintains its own users and roles using the standard asp.net membership and role providers.
        	
        TicketDesk includes a simple login and registration page as well as a simple User Management page (visible only to admin users) where the user role assignments can be changed. Users can also be deleted via the user management page.
        	
        The default database includes one user account:
        	
        User name = Admin
        Password = admin
        	
        You should create a new account, promote it to the admin role then remove the default admin account.
	
    AD Security: 

        TicketDesk can use windows authentication and AD groups instead of SQL server for security. 
        	
        TicketDesk uses the WindowsTokenRole provider when configured this way, but also requires you to configure it with an AD user and password that has at least read-access to the Active Directory domain. This is necessary because the default role provider does not expose a way to read group membership or fetch display and email address information from AD.
        	
        When using AD security, TicketDesk still requires and SQL Security database to store profile data, but user and groups will not be maintained within the application. You will have to manage the users and groups with Windows.
        	
        AD security reads the display name and email address for users as well as reading the list of members for the AD groups that are configured for use by TicketDesk.
        	
        The AD user account configured in web.config does NOT require any permissions to files or folders.
            	
	
Databases

    TicketDesk uses SQL Server 2005 databases and is compatible with either SQL Server Express 2005 or with full versions. 

    The default  configuration uses file attached SQL Express databases contained in the app_data folder. 

    If you are using a full edition of SQL server or if you desire a permanently attached SQL Express database, you can create the databases manually. 

    There is a SQL.txt file in the app_data folder that contains the table create scripts. Simply create your SQL database, run SQL.txt against it, then modify web.config's connection strings. 
     
    To create the security database you can use the aspnet_regsql tool (usually at: C:\Windows\Microsoft.NET\Framework\v2.0.50727)
    	
    You can also combine both the security and TicketDesk databases. Simply run SQL.txt then point aspnet_regsql at the same database. Then edit the TicketDesk connection string in web.config, remove the TicketDeskSecurity connection string, and edit the membership, profile, and role provider entries to point to the remaining TicketDesk connection string.
    	
    *NOTE: TicketDesk still requires a security database when using windows authentication and the AD role provider. This is necessary for some personalization information stored by the profile provider.
	
	






