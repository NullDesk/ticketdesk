define([
    'durandal/system',
    'services/model',
    'config',
    'services/logger',
    'services/datacontext'],
    function (system, model, config, logger, datacontext) {
        var activate = function() {
            var sig = $.connection.ticketDeskHub;
            $.connection.hub.start();
            logger.log('SignalR listening for ticket updates', null, system.getModuleId(datacontext), true);
            sig.client.ticketChanged = function(ticketId) {
                logger.log('Server says: ticket ' + ticketId + ' changed', null, 'home', true);
                datacontext.ticketEntityManager.refreshTicketById(ticketId);
            };
        };

        return {
            activate: activate
        };
    });