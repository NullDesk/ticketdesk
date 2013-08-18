define(function () {
    var remoteServiceName = 'breeze/ticketapi';

    var routes = [{
        url: 'home',
        name: $.i18n.t('appuitext:navMainHome'),//'Home',
        visible: true
    }, {
        url: 'tickets',
        name: $.i18n.t('appuitext:navMainTickets'),//'Tickets',
        moduleId: 'viewmodels/ticketlist',
        visible: true
    }, {
        url: 'ticket/new',
        moduleId: 'viewmodels/ticketcreate',
        name: $.i18n.t('appuitext:navMainTicketCreate'),
        visible: true
    }, {
        url: 'ticket/:id',
        moduleId: 'viewmodels/ticketdetails',
        name: $.i18n.t('appuitext:navMainTicketDetails'),//'Ticket Details',
        visible: false
    }];

    return {
        routes: routes,
        remoteServiceName: remoteServiceName
    };
});