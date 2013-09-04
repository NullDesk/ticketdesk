define(['durandal/system', 'services/datacontext', 'config', 'plugins/router', 'services/logger', 'services/notifiercontext'],
    function (system, datacontext, config, router, logger, notifiercontext) {
        var logoutUser = function () {
            return Q.when($.ajax({
                url: '/api/useraccount/logout',
                type: 'GET',
                contentType: 'application/json',
                data: JSON.stringify({ action: 'logout' })
            }))
                .then(function () { secureRoutes(false); })
                .then(datacontext.reset).then(notifiercontext.stopHubs)
                .fail(queryFailed);
        }

        var checkAuthentication = function () {
            return Q.when($.ajax({
                url: '/api/useraccount/authenticationcheck',
                type: 'GET',
                contentType: 'application/json'//,
                //data: JSON.stringify({ action: 'authenticationcheck' })
            }))
        }

        var loginUser = function (form, url) {

            return $.post(url, form.serialize(), function (data, status, response) {
                if (response.status === 200) {
                    secureRoutes(true);
                    datacontext.primeData().then(notifiercontext.startHubs);
                    router.navigate('');
                }
            });

            //var url = form.attr('action');
            //$.post(url, form.serialize(), function (data, status, response) {
            //    if (response.status === 200) {
            //        secureRoutes(true);
            //        datacontext.primeData();
            //        router.navigate('');
            //    }
            //}).fail(queryFailed);
        };

        var secureRoutes = function (isAuthenticated) {
            router.secureMappedRoutes(isAuthenticated).buildNavigationModel();
            return true;
        };

        var queryFailed = function (error, txt) {
            msg = txt;
            if (error.responseJSON) {
                var result = error.responseJSON.result;
                msg = 'server says: ' + result;
            }
            logger.logError(msg, error, system.getModuleId(vm), true);
            throw error;
        }

        var vm = {
            checkAuthentication: checkAuthentication,
            logoutUser: logoutUser,
            loginUser: loginUser
        };

        return vm;
    });