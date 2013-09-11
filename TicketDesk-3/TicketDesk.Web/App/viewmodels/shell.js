define(['durandal/system', 'plugins/router', 'config', 'services/account', 'services/datacontext', 'services/logger', 'services/notifiercontext', 'services/securitycontext'],
    function (system, router, config, account, datacontext, logger, notifiercontext, securitycontext) {
        var isAuthenticated = false;

        var shell = {
            router: router,
            activate: activate,
            attached: attached,
            isAuthenticated: isAuthenticated
           
        };

        return shell;
        

    

        function attached() {
            if (navigator.appVersion.indexOf("MSIE")) {
                elems = [$('body'), $('#applicationHost'), $('.durandal-wrapper'), $('html')];
                elems.forEach(function (i) { i.css('height', '100%').css('position', 'relative') });

            }
        }

        //#region Internal Methods

        function activate() {
            var self = this;
            
            return Q
                .when(account.checkAuthentication()
                        .then(function () {
                            self.isAuthenticated = true;
                        })
                        .fail(function () {
                            self.isAuthenticated = false;
                        }))
                .then(function () {
                    if (self.isAuthenticated) {
                        return datacontext.primeData()
                            .then(securitycontext.primeData)
                            .then(notifiercontext.startHubs)
                            .then(config.activate);
                    } else {
                        if (window.location.href.indexOf('#login') < 0) {
                            window.location.replace('/#login');
                        }
                        return config.activateForLogin();

                    }
                })
                .then(function () {
                    log('TicketDesk SPA Loaded!', null, true);
                })


        }


        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });