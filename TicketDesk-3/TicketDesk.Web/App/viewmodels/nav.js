
define(['plugins/router'], function (router) {

    //var adminRoutes = ko.computed(function () {
    //    return router.routes.filter(function (r) {
    //        return r.isAdmin;
    //    });
    //});
    var hasAdminRoutes = ko.computed(function () {
        return (router.adminNavigationModel().length > 0);
    });
    var collapseMenu = function () {
        if ($('#navMainButton').is(":visible")) {
            $("#navMain").collapse('hide');
        }
    };
    return {
        attached: function () {
            router.on('router:navigation:complete', collapseMenu);
        },
        router: router,
        hasAdminRoutes: hasAdminRoutes
    };
});
