/** 
 * @module Route table
 */
define(function () {

    var routes = {
		// Breeze Routes. Relative to entitymanagerprovider service name
        lookupUrl: "TicketDesk/lookups",
        saveChangesUrl: "TicketDesk/savechanges",
        ticketsUrl: "TicketDesk/tickets",
		
		//Authentication Routes
        addExternalLoginUrl : "/api/account/addexternallogin",
        changePasswordUrl : "/api/account/changepassword",
        loginUrl : "/token",
        logoutUrl : "/api/account/logout",
        registerUrl : "/api/account/register",
        registerExternalUrl : "/api/account/registerexternal",
        removeLoginUrl : "/api/account/removelogin",
        setPasswordUrl: "/api/account/setpassword",
        siteUrl : "/",
        userInfoUrl: "/api/account/userinfo",
        getUsersUrl : "/api/account/getusers"
    };

    return routes;

});