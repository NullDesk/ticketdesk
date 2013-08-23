
define(['plugins/router'], function (router) {


    var adminRoutes = ko.computed(function () {
        return router.routes.filter(function (r) {
            return r.isAdmin;
        });
    });

    var collapseMenu = function () {
        var navMain = $("#navMain");
        navMain.collapse('hide');
    };
    return {
        attached: function () {
            //$("#navMain").on("click", 'a', null, collapseMenu);
            router.on('router:navigation:complete', collapseMenu);
        },
        router: router,
        adminRoutes: adminRoutes
        
    };
});
