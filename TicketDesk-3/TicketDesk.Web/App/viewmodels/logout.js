define(['services/account', 'services/logger', 'plugins/router', 'config'],
    function (account, logger, router, config) {

        var compositionComplete = function () {
            logger.log('Logging out', null, 'login', false);
            return account.logoutUser().then(logoutRedirect);
        };

        var logoutRedirect = function () {
            //if this happens too fast, for some reason navigation hangs. 
            //  Only seen when using main-built.js though
            window.setTimeout(function () { router.navigate('') }, 1000);
        };

        return {
            compositionComplete: compositionComplete,
            title: $.i18n.t('appuitext:viewLogoutTitle'),
        };

    });


