/** 
  * @module Manage registering users
  * @requires appsecurity
  * @requires router
  * @requires errorHandler
*/

define(['services/appsecurity', 'plugins/router', 'services/errorhandler'],
    function (appsecurity, router, errorhandler) {

        var username = ko.observable().extend({ required: true }),
            email = ko.observable().extend({ required: true, email : true }),
            password = ko.observable().extend({ required: true, minLength: 6 }),
            confirmpassword = ko.observable().extend({ required: true, minLength: 6, equal : password });
        
        var viewmodel =  {
            
            username : username,
            
            email : email,
            
            password :  password,
            
            confirmpassword : confirmpassword,
            
            activate : function () {
                ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
            },

            register : function () {
                var self = this;                
                if (this.errors().length != 0) {                    
                    this.errors.showAllMessages();
                    return;
                }

				appsecurity.register({
					userName: self.username(),
					eMail : self.email(),
					password: self.password(),
					confirmPassword: self.confirmpassword()
				}).done(function (data) {
					appsecurity.login({
						grant_type: "password",
						username: self.username(),
						password: self.password()
					}).done(function (data) {
						if (data.userName && data.access_token) {
							appsecurity.setAuthInfo(
								data.userName, 
								data.roles, 
								data.access_token, 
								self.rememberMe);
							self.username("");
							self.email("");
							self.password("");
							self.confirmpassword("");
							self.errors.showAllMessages(false);
						}
						router.navigate("account/manage");
					}).fail(self.handleauthenticationerrors);
				}).fail(self.handlevalidationerrors);				
            }
        }

        errorhandler.includeIn(viewmodel);

        viewmodel["errors"] = ko.validation.group(viewmodel);

        return viewmodel;
});