define(['services/account', 'services/logger'],
    function (account, logger) {
        var username = ko.observable("admin");

        var password = ko.observable("");

        var isValid = ko.computed(function () {
            return username() && password();
        });

        //#region Internal Methods

        var activate = function () {
            logger.log('Login View Activated', null, 'login', true);
            return true;
        };

        var loginUser = function () {
            if (!isValid()) {
                return Q.resolve(false);
            }
            return account.loginUser(username(), password())
                .then(function () {
                    window.location = '/';
                    return true;
                });
                
        };
        var keyMonitor = function (data, event) {
            if (event.keyCode == 13) {
                $('#loginbutton').focus();
                loginUser();
            }
            return true;
        };

        //#endregion
        var vm = {
            keyMonitor: keyMonitor,
            activate: activate,
            title: $.i18n.t('appuitext:viewLoginTitle'),
            username: username,
            password: password,
            loginUser: loginUser,
            isValid: isValid
        };
        return vm;

    });


