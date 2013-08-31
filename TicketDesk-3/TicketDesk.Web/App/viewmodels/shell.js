define(['durandal/system', 'plugins/router', 'config', 'services/datacontext', 'services/notifiercontext', 'services/logger'],
    function (system, router, config, datacontext, notifiercontext, logger) {

        var shell = {
            router: router,
            activate: activate
        };

        return shell;

        //#region Internal Methods
        function activate() {
            $.extend(router, {
                loginModel: ko.observableArray([]),
                buildLoginModel: function (defaultOrder) {

                    var addActiveFlag = function (config, r) {
                        if (config.isActive) {
                            return;
                        }

                        config.isActive = ko.computed(function () {
                            var theItem = r.activeItem();
                            return theItem && theItem.__moduleId__ == config.moduleId;
                        });
                    };
                    var lmod = [], routes = router.routes;
                    defaultOrder = defaultOrder || 100;

                    for (var i = 0; i < routes.length; i++) {
                        var current = routes[i];

                        if (current.account) {
                            if (!system.isNumber(current.lmod)) {
                                current.lmod = defaultOrder;
                            }


                            addActiveFlag(current, this);

                            lmod.push(current);
                        }
                    }

                    lmod.sort(function (a, b) { return a.account - b.account; });
                    router.loginModel(lmod);

                    return router;
                }
            });
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
                    log('Login Mode Activated!', null, true);
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