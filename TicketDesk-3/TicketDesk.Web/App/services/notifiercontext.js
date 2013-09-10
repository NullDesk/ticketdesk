define([
    'durandal/system',
    'services/model',
    'config',
    'services/logger',
    'services/datacontext'],
    function (system, model, config, logger, datacontext) {


        var stopHubs = function () {
            return Q.when(function() {
                $.connection.hub.stop();
                logger.log('SignalR no longer listening for ticket updates', null, system.getModuleId(datacontext), true);
            });
        };
        var startHubs = function () {
            return Q.when(function() {
                var sig = $.connection.ticketDeskHub;
                $.connection.hub.start();
                logger.log('SignalR listening for ticket updates', null, system.getModuleId(datacontext), true);
                sig.client.ticketChanged = function(ticketId) {
                    logger.log('Server says: ticket ' + ticketId + ' changed', null, 'home', true);
                    datacontext.refreshTicketById(ticketId);
                };
            });
        };

        var activate = function() {
            var x = datacontext;
        };

        return {
            activate: activate,
            stopHubs: stopHubs,
            startHubs: startHubs
        };
    });