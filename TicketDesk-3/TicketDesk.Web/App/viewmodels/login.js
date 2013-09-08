define(['durandal/system', 'services/account', 'plugins/router', 'services/logger'],
    function (system, account, router, logger) {


        var keyMonitor = function (data, event) {
            if (event.keyCode == 13) {
                $('#loginbutton').focus();
                doLogin();
            }
            return true;
        };

        var doLogin = function () {
            var form = $("#loginTarget").contents().find("#loginForm");
            var u = $('#username').val();
            var p = $('#password').val();
            var r = $('#rememberme').val();
            form.find("#username").val(u);
            form.find("#password").val(p);
            form.submit();

            account.loginUser(u, p, r)
                .fail(function () {
                    logger.logError($.i18n.t('appuitext:viewLoginFailedErrorMessage'), null, system.getModuleId(vm), true);

                });
        };

        window.formLoaded = function () {

        };
        var attached = function () {
            $("#loginTarget").ready(window.setTimeout(function () {
                var form = $("#loginTarget").contents().find("#loginForm");
                $('#username').val(form.find("#username").val());
                $('#password').val(form.find("#password").val());
            }, 100));
          
        };
        var deactivate = function () {
            $('#username').val('');
            $('#password').val('');
        };

        var vm = {
            deactivate: deactivate,
            attached: attached,
            keyMonitor: keyMonitor,

            doLogin: doLogin,
            title: $.i18n.t('appuitext:viewLoginTitle'),
        };
        return vm;

    });


