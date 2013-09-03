define(['plugins/router',  'util/routerExtension'], function (router,routerExtension) {
    var remoteServiceName = 'breeze/ticketapi';

    var mainMap = [{
        route: '',
        moduleId: 'home',
        title: $.i18n.t('appuitext:navMainHome'),
        nav: true,
        allowAnonymous: true,
        button: '<i class="icon-home"></i> ' + $.i18n.t('appuitext:navMainHome')
    }, {
        route: 'tickets',
        moduleId: 'ticketlist',
        title: $.i18n.t('appuitext:navMainTickets'),
        nav: true,
        button: '<i class="icon-tasks"></i> ' + $.i18n.t('appuitext:navMainTickets')
    }, {
        route: 'ticket/new',
        moduleId: 'ticketcreate',
        title: $.i18n.t('appuitext:navMainTicketCreate'),
        nav: true,
        button: '<i class="icon-plus-sign"></i> ' + $.i18n.t('appuitext:navMainTicketCreate')
    }, {
        route: 'ticket/:id',
        moduleId: 'ticketdetails',
        title: $.i18n.t('appuitext:navMainTicketDetails'),
        nav: false
    }, {
        route: '',
        moduleId: 'home',
        title: 'some admin page',
        nav: false,
        isAdmin: ko.observable(true)
    }, {
        route: 'logout',
        moduleId: 'logout',
        title: $.i18n.t('appuitext:navAccountLogout'),
        nav: false,
        isAccount: ko.observable(true),
        hideOnAuthenticate: false,
        button: '<i class="icon-signout"></i> ' + $.i18n.t('appuitext:navAccountLogout')
    }, {
        route: 'login',
        moduleId: 'login',
        title: $.i18n.t('appuitext:navAccountLogin'),
        nav: false,
        isAccount: ko.observable(true),
        hideOnAuthenticate: true,
        button: '<i class="icon-signin"></i> ' + $.i18n.t('appuitext:navAccountLogin')
    }];

    var initRoutes = function(mapSet, isAuthenticated) {
        routerExtension.extendRouter();
        return router.makeRelative({ moduleId: 'viewmodels' })
            .map(mapSet)
            .initMappedRoutes()
            .secureMappedRoutes(isAuthenticated)
            .buildNavigationModel()
            .mapUnknownRoutes('login', 'login').activate();
    }
    var config = {
        remoteServiceName: remoteServiceName,
        router: router,
        activateForLogin: function () {
            return initRoutes(mainMap, false);
        },
        activate: function () {
            return initRoutes(mainMap, true);
        }
    };
    return config;
});