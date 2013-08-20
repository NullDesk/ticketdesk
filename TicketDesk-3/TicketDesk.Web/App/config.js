define(function () {
    var remoteServiceName = 'breeze/ticketapi';

    var routes = [{
        url: 'home',
        name: $.i18n.t('appuitext:navMainHome'),//'Home',
        visible: true,
        caption: '<i class="icon-home"></i> ' + $.i18n.t('appuitext:navMainHome')
    }, {
        url: 'tickets',
        name: $.i18n.t('appuitext:navMainTickets'),//'Tickets',
        moduleId: 'viewmodels/ticketlist',
        visible: true,
        caption: '<i class="icon-tasks"></i> ' + $.i18n.t('appuitext:navMainTickets')
    }, {
        url: 'ticket/new',
        moduleId: 'viewmodels/ticketcreate',
        name: $.i18n.t('appuitext:navMainTicketCreate'),
        visible: true,
        settings: { inverse: true },
        caption: '<i class="icon-plus-sign"></i> ' + $.i18n.t('appuitext:navMainTicketCreate')
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