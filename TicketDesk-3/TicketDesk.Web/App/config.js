define(['plugins/router'], function (router) {

    var remoteServiceName = 'breeze/ticketapi';

    var mapLogin = function (r) {
        return r.map([{
            route: 'login',
            moduleId: 'login',
            title: $.i18n.t('appuitext:navLoginHome'),
            nav: false,
            isAccount: true,
            button: '<i class="icon-signin"></i> ' + $.i18n.t('appuitext:navLoginHome')
        }]);
    }

    var mapMain = function (r) {
        return r.map([{
            route: '',
            moduleId: 'home',
            title: $.i18n.t('appuitext:navMainHome'),
            nav: true,
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
            isAdmin: true
        }]);
    }

    var initRoutes = function (mapFn, initialRoute) {
        router.reset();
         var r = router.makeRelative({ moduleId: 'viewmodels' })
         return mapFn(r).buildNavigationModel()
          .mapUnknownRoutes('login', 'login')
          .activate();
    }

    return {
        remoteServiceName: remoteServiceName,
        router: router,
        activateForLogin: function () {
            return initRoutes(mapLogin);
        },
        activate: function (mapMain) {
            return initRoutes(mapMain);
        }
    };
});