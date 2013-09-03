define(['durandal/system', 'plugins/router', 'config', 'services/account', 'services/datacontext', 'services/logger'],
    function (system, router, config, account, datacontext, logger) {

        var shell = {
            router: router,
            activate: activate
        };

        return shell;

        //#region Internal Methods
        function activate() {

            return account.checkAuthentication()
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
                    log('Login Mode Activated!', null, false);
                });
        }
        function boot() {
            return config.activate()
                .then(datacontext.primeData()).then(function () { log('TicketDesk SPA Loaded!', null, false); });

        }
        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });