define(['durandal/system','plugins/router', 'config', 'services/datacontext', 'services/notifiercontext','services/logger'],
    function (system, router, config, datacontext, notifiercontext, logger) {
        
        var shell = {
            router: router,
            activate: activate
        };

        return shell;

        //#region Internal Methods
        function activate() {
            return datacontext.primeData().then(boot).then(notifiercontext.activate);
        }

        function boot() {
            return config.activate().then(function() {
                log('TicketDesk SPA Loaded!', null, true);
            });
        }
        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });