define(['plugins/router'], function (router) {
    
    var remoteServiceName = 'breeze/ticketapi';


    return {
        remoteServiceName:remoteServiceName,
        router: router,
        activate: function () {
            return router.makeRelative({ moduleId: 'viewmodels' }).map([{
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
            }]).buildNavigationModel()
              .mapUnknownRoutes('home', 'home')
              .activate();
        }
    };
});