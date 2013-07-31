define(function() {
    var remoteServiceName = 'breeze/ticketapi';

    var routes = [{
            url: 'home',
            name: $.i18n.t('navMainHome'),//'Home',
            visible: true
        },{
            url: 'tickets',
            name: $.i18n.t('navMainTickets'),//'Tickets',
            moduleId: 'viewmodels/ticketlist',
            visible: true
        },{
            url: 'ticket/:id',
            moduleId: 'viewmodels/ticketdetails',
            name: $.i18n.t('navMainTicketDetails'),//'Ticket Details',
            visible: false
        }];

    return {
        routes: routes,
        remoteServiceName: remoteServiceName
    };
});