
define(['durandal/system','plugins/router', 'services/logger', 'durandal/composition'], function (system, router, logger) {
    var hasAdminRoutes = ko.computed(function () {
        return (router.adminNavigationModel().length > 0);
    });
    
    var navComplete = function (instance, instruction, router) {
        if ($('#navMainButton').is(":visible")) {
            $("#navMain").collapse('hide');
        }
    };
    
    var nav = {
        attached: function () {
            router.on('router:navigation:composition-complete', navComplete);
        },
        router: router,
        hasAdminRoutes: hasAdminRoutes
    };
    return nav;
    
    function log(msg, data, showToast) {
        logger.log(msg, data, system.getModuleId(nav), showToast);
    }
});
