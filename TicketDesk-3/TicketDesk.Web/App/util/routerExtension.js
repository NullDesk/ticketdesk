define(['plugins/router'],
    function (router) {
        var extension = {
            accountNavigationModel: ko.observableArray([]),
            adminNavigationModel: ko.observableArray([]),
            initMappedRoutes: function () {
                $.each(this.routes, function (idx, route) {
                    if (route.nav) {
                        route.isNavOnAuthenticate = route.nav;
                    }
                    if (('isAdmin' in route) && route.isAdmin()) {
                        route.isAdminOnAuthenticate = route.isAdmin() && !route.hideOnAuthenticate;
                    }
                    if (('isAccount' in route) && route.isAccount()) {
                        route.isAccountOnAuthenticate = route.isAccount() && !route.hideOnAuthenticate;
                    }

                });
                return this;
            },
            secureMappedRoutes: function (isAuthenticated) {
                $.each(this.routes, function (idx, route) {
                    route.nav = (route.allowAnonymous || (isAuthenticated && route.isNavOnAuthenticate));
                    if ('isAccount' in route) {
                        var vis = (isAuthenticated == route.isAccountOnAuthenticate);
                        route.isAccount(vis);
                    }
                    if ('isAdmin' in route) {
                        var vis = (isAuthenticated == route.isAdminOnAuthenticate);
                        route.isAdmin(vis);
                    }
                });
                this.buildAccountNavigationModel();
                this.buildAdminNavigationModel();
                return this;
            },
            buildSecondaryNavigationModel: function(routePropertyName, targetObservable){
                var anav = [], routes = router.routes;
                for (var i = 0; i < routes.length; i++) {
                    var current = routes[i];
                    if ((routePropertyName in current) && current[routePropertyName]()) {
                        addActiveFlag(current);
                        anav.push(current);
                    }
                }

                function addActiveFlag(config) {
                    if (config.isActive) {
                        return;
                    }

                    config.isActive = ko.computed(function () {
                        var theItem = router.activeItem();
                        return theItem && theItem.__moduleId__ == config.moduleId;
                    });
                }

                // accnav.sort(function (a, b) { return a.nav - b.nav; });
                targetObservable(anav);

                return router;
            },

            buildAccountNavigationModel: function () {
                this.buildSecondaryNavigationModel('isAccount', this.accountNavigationModel)
            },

            buildAdminNavigationModel: function () {
                this.buildSecondaryNavigationModel('isAdmin', this.adminNavigationModel)
            },
                //var anav = [], routes = router.routes;

                //for (var i = 0; i < routes.length; i++) {
                //    var current = routes[i];

                //    if (('isAccount' in current) && current.isAccount()) {
                //        addActiveFlag(current);
                //        anav.push(current);
                //    }
                //}

                //function addActiveFlag(config) {
                //    if (config.isActive) {
                //        return;
                //    }

                //    config.isActive = ko.computed(function () {
                //        var theItem = router.activeItem();
                //        return theItem && theItem.__moduleId__ == config.moduleId;
                //    });
                //}

                //// accnav.sort(function (a, b) { return a.nav - b.nav; });
                //router.accountNavigationModel(anav);

                //return router;
               
        };




        var routerExtension = {
            extendRouter: function () {
                $.extend(router, extension);
            },
        };

        return routerExtension;
    });