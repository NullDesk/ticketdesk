define(['durandal/system', 'plugins/router', 'config', 'services/datacontext', 'services/notifiercontext', 'services/logger'],
    function (system, router, config, datacontext, notifiercontext, logger) {

        var shell = {
            router: router,
            activate: activate
        };

        return shell;

        //#region Internal Methods
        function activate() {
            return datacontext.primeData()
                .then(boot)
                .fail(function (e) {
                    if (e.status === 401) {
                        return bootToLogin();
                    } else {
                        log(e.message, null, true);
                        return false;
                    }
                });
        }
        function bootToLogin() {
            if (!window.location.href.indexOf('#login')) {
                window.location.replace('/#login')
            }
            return config.activateForLogin()
                .then(function () {
                    log('TicketDesk SPA Login Mode Activated!', null, true);
                });
        }
        function boot() {
            return config.activate()
                .then(notifiercontext.activate)
                .then(function () {
                    log('TicketDesk SPA Loaded!', null, true);
                });

        }
        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });