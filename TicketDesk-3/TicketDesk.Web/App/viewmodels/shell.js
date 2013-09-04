define(['durandal/system', 'plugins/router', 'config', 'services/account', 'services/datacontext', 'services/logger', 'services/notifiercontext'],
    function (system, router, config, account, datacontext, logger, notifiercontext) {

        var shell = {
            router: router,
            activate: activate,
            attached: attached
        };

        return shell;

        function attached() {
            if (navigator.appVersion.indexOf("MSIE")) {
                elems = [ $('body'), $('#applicationHost'), $('.durandal-wrapper'), $('html') ];
                elems.forEach(function(i){i.css('height', '100%').css('position', 'relative')});

            }
        }

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
                .then(datacontext.primeData())
                .then(notifiercontext.startHubs)
                .then(function () { log('TicketDesk SPA Loaded!', null, false); });

        }
        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });