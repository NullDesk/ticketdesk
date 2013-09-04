define(['durandal/system', 'services/account', 'plugins/router', 'services/logger'],
    function (system, account, router, logger) {
        var iframeDefaultSource;
        var doSubmit = function (form) {
            var url = 'api/useraccount/login';

            account.loginUser($(form), url).fail(function () {
                logger.logError("Login failed", null, system.getModuleId(vm), true);
                var targ = $("#loginTarget");
                targ.get(0).contentDocument.location.replace(iframeDefaultSource);
            });
            return true;
        }
        var setLoginIframeHeight = function () {
           var iframe = $("#loginTarget").get(0);
            if (iframe) {
                var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
                if (iframeWin.document.body) {
                    iframe.height = iframeWin.document.documentElement.scrollHeight || iframeWin.document.body.scrollHeight;
                }
            }
        };

        var attached = function () {
            iframe = $("#loginTarget");
            iframeDefaultSource = iframe.attr('src');
            window.doSubmit = doSubmit;
            window.setLoginIframeHeight = setLoginIframeHeight;
            $(window).resize(setLoginIframeHeight);
            
        };
        var vm = {
            attached: attached,
            title: $.i18n.t('appuitext:viewLoginTitle'),
        };
        return vm;

    });


