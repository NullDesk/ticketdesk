
define(['plugins/router'], function (router) {


    var accountRoutes = ko.computed(function () {
        return router.routes.filter(function (r) {
            return r.isAccount;
        });
    });

    

    var adminRoutes = ko.computed(function () {
        return router.routes.filter(function (r) {
            return r.isAdmin;
        });
    });
    var hasAdminRoutes = ko.computed(function () {
        return (adminRoutes().length > 0) && (!window.location.href.indexOf('#login'));
    });
    var collapseMenu = function () {
        if ($('#navMainButton').is(":visible")) {
            $("#navMain").collapse('hide');
        }
    };
    return {
        attached: function () {
            //$("#navMain").on("click", 'a', null, collapseMenu);
            router.on('router:navigation:complete', collapseMenu);
        },
        router: router,
        hasAdminRoutes: hasAdminRoutes,
        adminRoutes: adminRoutes,
        accountRoutes: accountRoutes
    };
});
