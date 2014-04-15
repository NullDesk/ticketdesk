define(['plugins/router', 'services/appsecurity', 'services/errorhandler', 'services/entitymanagerprovider', 'model/modelBuilder', 'services/appsecurity', 'i18next'],
    function (router, appsecurity, errorhandler, entitymanagerprovider, modelBuilder, appsecurity, i18n) {

        entitymanagerprovider.modelBuilder = modelBuilder.extendMetadata;

        var viewmodel = {

            attached: function () {
                $(document).find("footer").show();

            },

            activate: function () {
                var self = this;

                return entitymanagerprovider
                        .prepare()
                        .then(function () {

                            //configure routing
                            router.makeRelative({ moduleId: 'viewmodels' });

                            // If the route has the authorize flag and the user is not logged in => navigate to login view                                
                            router.guardRoute = function (instance, instruction) {
                                if (sessionStorage["redirectTo"]) {
                                    var redirectTo = sessionStorage["redirectTo"]
                                    sessionStorage.removeItem("redirectTo");
                                    return redirectTo;
                                }

                                if (instruction.config.authorize) {
                                    if (typeof (appsecurity.userInfo()) !== 'undefined') {
                                        if (appsecurity.isUserInRole(instruction.config.authorize)) {
                                            return true;
                                        } else {
                                            return "/account/login?returnUrl=" + encodeURIComponent(instruction.fragment);
                                        }
                                    } else {
                                        return "/account/login?returnUrl=" + encodeURIComponent(instruction.fragment);
                                    }
                                } else {
                                    return true;
                                }
                            };

                            // Config Routes
                            // Routes with authorize flag will be forbidden and will redirect to login page
                            // As this is javascript and is controlled by the user and his browser, the flag is only a UI guidance. You should always check again on 
                            // server in order to ensure the resources travelling back on the wire are really allowed

                            return router.map([
                                // Nav urls
                                { route: ['', 'home/index'], moduleId: 'home/index', title: 'Home', nav: true, hash: "#home/index" },
                                { route: 'home/tickets', moduleId: 'home/ticketlist', title: 'Tickets', nav: true, hash: "#home/tickets" },

                                { route: 'home/help', moduleId: 'home/help', title: 'Help', nav: true, hash: "#home/help" },
                                { route: 'home/about', moduleId: 'home/about', title: 'About', nav: true, hash: "#home/about" },
                                { route: 'notfound', moduleId: 'notfound', title: 'Not found', nav: false },

                                // Admin panel url
                                { route: 'admin/panel', moduleId: 'admin/panel', title: 'Admin Panel', nav: false, hash: "#admin/panel", authorize: ["Administrator"] },

                                // Account Controller urls
                                { route: 'account/login', moduleId: 'account/login', title: 'Login', nav: false, hash: "#account/login" },
                                { route: 'account/externalloginconfirmation', moduleId: 'account/externalloginconfirmation', title: 'External login confirmation', nav: false, hash: "#account/externalloginconfirmation" },
                                { route: 'account/externalloginfailure', moduleId: 'account/externalloginfailure', title: 'External login failure', nav: false, hash: "#account/externalloginfailure" },
                                { route: 'account/register', moduleId: 'account/register', title: 'Register', nav: false, hash: "#account/register" },
                                { route: 'account/manage', moduleId: 'account/manage', title: 'Manage account', nav: false, hash: "#account/manage", authorize: ["User", "Administrator"] },

                                // User articles urls
                               ])
                            .buildNavigationModel()
                            .mapUnknownRoutes("notfound", "notfound")
                            .activate({ pushState: true });
                        })
                        .fail(self.handlevalidationerrors);
            }
        };

        errorhandler.includeIn(viewmodel);

        return viewmodel;
    });