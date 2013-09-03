define(['durandal/system', 'services/account', 'plugins/router', 'services/logger'],
    function (system, account, router, logger) {
        //var username = ko.observable("admin");

        //var password = ko.observable("");

        //var isValid = ko.computed(function () {
        //    return username() && password();
        //});

        ////#region Internal Methods

        //var activate = function () {
        //    logger.log('Login View Activated', null, 'login', true);
        //    return true;
        //};
        //var deactivate = function () {
        //    password('');
        //}

        //var loginUser = function (e) {
        //    e.preventDefault();
        //    if (!isValid()) {
        //        return Q.resolve(false);
        //    }
        //    account.loginUser(username(), password())
        //        .then(function () {
        //            router.navigate('');
        //            return true;
        //        });
        //    return false;
        //};
        //var keyMonitor = function (data, event) {
        //    if (event.keyCode == 13) {
        //        $('#loginbutton').focus();
        //        //loginUser();
        //    }
        //    return true;
        //};

        //var AddAntiForgeryToken = function (data) {
        //    data.__RequestVerificationToken = $('#loginForm input[name=__RequestVerificationToken]').val();
        //    return data;
        //};
        //var attached = function () {
        //    var iframe = $('#loginTarget');
        //    var form = $('#loginForm');
        //    iframe.appendTo('#loginArea').show();
        //    //ko.cleanNode(form.get(0));
        //    //ko.applyBindings(vm, form.get(0));
        //    form.submit(function (e) {
        //        e.preventDefault();
        //        $.post('/account/login', AddAntiForgeryToken($(this).serialize()), function (data, status, response) {
        //            if (response.status === 200) {
        //                window.location = '';
        //            }
        //        });

        //        return false;
        //    })

        //};

        //#endregion

        var bindSubmit = function () {
            var form = $("#loginTarget").contents().find("#loginForm")
            form.submit(doSubmit);
        };

        var iframeDefaultSource;
        var doSubmit = function (e) {
            //e.preventDefault();
            var url = 'api/useraccount/login';


            //$.post(url, $(this).serialize(), function (data, status, response) {
            //    if (response.status === 200) {
            //        router.navigate('');
            //    }
            //})

            account.loginUser($(this), url).fail(function () {
                logger.logError("Login failed", null, system.getModuleId(vm), true);
                var targ = $("#loginTarget");
                targ.get(0).contentDocument.location.replace(iframeDefaultSource);
                
            });

            return true;
        }


        var attached = function () {
            iframeDefaultSource = $("#loginTarget").attr('src');
            window.bindLoginSubmit = bindSubmit;
        };
        var vm = {
            attached: attached,
            //keyMonitor: keyMonitor,
            //activate: activate,
            title: $.i18n.t('appuitext:viewLoginTitle'),
            //username: username,
            //password: password,
            //loginUser: loginUser,
            //isValid: isValid,
            //deactivate: deactivate
        };
        return vm;

    });


