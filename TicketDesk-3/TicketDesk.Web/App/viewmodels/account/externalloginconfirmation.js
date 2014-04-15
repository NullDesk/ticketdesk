/** 
    * @module Confirm an external login
    * @requires appsecurity
    * @requires router
    * @requires errorHandler
*/

define(['services/appsecurity', 'plugins/router', 'services/errorhandler', 'services/utils'],
function (appsecurity,router,errorhandler,utils) {

    var loginProvider = ko.observable(),
        userName = ko.observable().extend({ required: true }),
        email = ko.observable().extend({ required: true, email:true }),
        externalAccessToken = ko.observable(),
        loginUrl = ko.observable(),
		state = ko.observable()
    
    var viewmodel = {
        /** @property {observable} loginProvider */
        loginProvider: loginProvider,

        /** @property {observable} userName */
        userName: userName,

        /** @property {observable} eMail - Email for the new user */
        email: email,

        /**
         * Activate view
         * @method
        */
        activate: function () {
            var self = this;
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });

            self.loginProvider(utils.getUrlParameter("loginProvider"));
            self.userName(utils.getUrlParameter("userName"));
            externalAccessToken = utils.getUrlParameter("access_token");
            loginUrl = decodeURIComponent(utils.getUrlParameter("loginUrl"));
            state = utils.getUrlParameter("state");

            return true;
        },

        /**
         * Confirm an exernal account
         * @method
        */
        confirmexternalaccount: function () {
            var self = this;

            if (this.errors().length != 0) {
                this.errors.showAllMessages();
                return;
            }

            appsecurity.registerExternal(externalAccessToken, {
                userName: self.userName(),
                email: self.email()
            }).done(function (data) {
                sessionStorage["state"] = state;
                // IE doesn't reliably persist sessionStorage when navigating to another URL. Move sessionStorage
                // temporarily to localStorage to work around this problem.
                appsecurity.archiveSessionStorageToLocalStorage();
                window.location = loginUrl;
            }).fail(self.handlevalidationerrors);
        }
    }
    
    errorhandler.includeIn(viewmodel);
    
    viewmodel["errors"] = ko.validation.group(viewmodel);
    
    return viewmodel
    
});