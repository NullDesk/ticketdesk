define(['plugins/router', 'services/appsecurity', 'services/errorhandler'],
    function (router, appsecurity, errorhandler) {

        var viewmodel = {
            attached: function (view, parent) {
                //$(view).i18n();
            },

            router: router,

            appsecurity: appsecurity,

            logout: function () {
                var self = this;
                appsecurity.logout()
                    .done(function () {
                        appsecurity.clearAuthInfo();
                        if (router.activeInstruction().config.authorize) {
                            router.navigate("account/login");
                        }
                    })
                    .fail(self.handlevalidationerrors);
            }
        };

        errorhandler.includeIn(viewmodel);

        return viewmodel;
    });