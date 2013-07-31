define(['durandal/system', 'durandal/plugins/router', 'services/logger', 'config', 'services/datacontext', 'services/notifiercontext'],
    function (system, router, logger, config, datacontext, notifiercontext) {
        var shell = {
            activate: activate,
            router: router
        };
        
        return shell;

        //#region Internal Methods
        function activate() {
            return datacontext.primeData().then(notifiercontext.activate).then(boot);
        }

        function boot() {

            router.map(config.routes);
            log('TicketDesk SPA Loaded!', null, true);
            return router.activate('home');
        }

        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });