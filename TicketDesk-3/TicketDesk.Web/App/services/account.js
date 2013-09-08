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
                contentType: 'application/json'
            }))
        }

        var loginUser = function (username, password, rememberme) {
            var data = {
                username: username,
                password: password,
                rememberme: rememberme
            };

            return Q.when($.ajax({
                url: '/api/useraccount/login',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(data)
            }))
                .then(function () {
                    secureRoutes(true);
                })
                .then(datacontext.primeData)
                .then(notifiercontext.startHubs)
                .then(function () {
                    router.navigate('');
                })
                .fail(queryFailed);
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