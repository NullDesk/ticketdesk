/** 
 * @module Login
 * @requires appsecurity
 * @requires router
 * @requires errorHandler
 */

define(['services/appsecurity', 'plugins/router', 'services/errorhandler'],
    function (appsecurity, router, errorhandler) {

        var username = ko.observable().extend({ required: true }),
            password = ko.observable().extend({ required: true, minLength: 4 }),
            rememberMe = ko.observable(false),
            returnUrl = ko.observable(null),
            isAuthenticated = ko.observable(false);

        function ExternalLoginProviderViewModel(data, returnUrl) {
            var self = this;

            self.name = ko.observable(data.name);
            
            self.login = function () {
                sessionStorage["state"] = data.state;
                sessionStorage["loginUrl"] = data.url;
                if (returnUrl) {
                    sessionStorage["redirectTo"] = returnUrl;
                } else {
                    sessionStorage["redirectTo"] = "account/manage";
                }

                // IE doesn't reliably persist sessionStorage when navigating to another URL. Move sessionStorage temporarily
                // to localStorage to work around this problem.
                appsecurity.archiveSessionStorageToLocalStorage();                

                window.location = data.url;
            };

            self.socialIcon = function(data) {
                var icon = "";
                switch (data.name().toLowerCase()) {
                case "facebook":
                    icon = "fa fa-facebook-square";
                    break;
                case "twitter":
                    icon = "fa fa-twitter-square";
                    break;
                case "google":
                    icon = "fa fa-google-plus-square";
                    break;
                case "microsoft":
                    icon = "fa fa-envelope";
                    break;
                default:
                    icon = "fa fa-check-square";
                }
                return icon;
            };
        }

        var viewmodel =  {
            
            convertRouteToHash: router.convertRouteToHash,
            
            username : username,
            
            password : password,
            
            rememberMe : rememberMe,
            
            returnUrl : returnUrl,       
            
            appsecurity: appsecurity,

            externalLoginProviders: ko.observableArray(),
  
            activate: function (splat) {
                var self = this;

                ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });

                if (splat && splat.returnUrl) {
                    self.returnUrl(splat.returnUrl);
                }                
                
                return appsecurity.getExternalLogins(appsecurity.returnUrl, true)
                    .then(function (data) {
                        if (typeof (data) === "object") {
                            self.externalLoginProviders.removeAll();
                            for (var i = 0; i < data.length; i++) {
                                self.externalLoginProviders.push(new ExternalLoginProviderViewModel(data[i], self.returnUrl() ? self.returnUrl() : null ));
                            }
                        }
                    }).fail(self.handleauthenticationerrors);                
            },

            login: function () {
                var self = this;

                if (this.errors().length != 0) {
                    this.errors.showAllMessages();
                    return;
                }

                appsecurity.login({
                    grant_type: "password",
                    username: self.username(),
                    password: self.password()
                }).done(function (data) {
                    if (data.userName && data.access_token) {
                        appsecurity.setAuthInfo(data.userName, data.roles, data.access_token, self.rememberMe());

                        // get the current default Breeze AJAX adapter
                        var ajaxAdapter = breeze.config.getAdapterInstance("ajax");
                        // set fixed headers
                        ajaxAdapter.defaultSettings = {
                            headers: appsecurity.getSecurityHeaders()
                        };

                        self.username("");
                        self.password("");
                        self.rememberMe(false);

                        self.errors.showAllMessages(false);

                        if (self.returnUrl()) {
                            router.navigate(self.returnUrl());
                        } else {
                            router.navigate("account/manage");
                        }
                    }
                }).fail(self.handleauthenticationerrors);
            },

            logout: function () {
                appsecurity.logout();
            }
        }

        errorhandler.includeIn(viewmodel);

        viewmodel["errors"] = ko.validation.group(viewmodel);

        return viewmodel;
    });