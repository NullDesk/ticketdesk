define(['durandal/system', 'services/logger'],
    function (system, logger) {
        var logoutUser = function () {
            return Q.when($.ajax({
                url: '/api/useraccount/logout',
                type: 'GET',
                contentType: 'application/json',
                data: JSON.stringify({action: 'logout'})
            })).fail(queryFailed);

            
        };

        var loginUser = function (username, password) {
            var data = {
                username: username,
                password: password
            };

            return Q.when($.ajax({
                url: '/api/useraccount/login',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(data)
            })).fail(queryFailed);
        };

        function queryFailed(error, txt) {
            msg = txt;
            if (error.responseJSON) {
                var result = error.responseJSON.result;
                msg = 'server says: ' + result;
            }
            logger.logError(msg, error, system.getModuleId(vm), true);
            throw error;
        }

        var vm = {
            logoutUser: logoutUser,
            loginUser: loginUser
        };

        return vm;
    });