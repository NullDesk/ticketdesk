define(function() {
    var remoteServiceName = 'breeze/ticketapi';

    var routes = [{
            url: 'home',
            name: 'Home',
            visible: true
        },{
            url: 'tickets',
            name: 'Tickets',
            moduleId: 'viewmodels/ticketlist',
            visible: true
        },{
            url: 'ticket/:id',
            moduleId: 'viewmodels/ticketdetails',
            name: 'Ticket Details',
            visible: false
        }];

    return {
        routes: routes,
        remoteServiceName: remoteServiceName
    };
});