define(['services/account', 'services/logger', 'plugins/router', 'config'],
    function (account, logger, router, config) {

        var attached = function () {
            logger.log('Logging out', null, 'login', true);
            return account.logoutUser().then(logoutRedirect);
        };

        var logoutRedirect = function () {
            //router.splice(0, router.routes.length);
            config.activateForLogin().then(function () {
                window.location = '/';
                
            });
        };



        return {
            attached: attached,
            title: $.i18n.t('appuitext:viewLogoutTitle'),
        };

    });


